using System;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Manager.Core.Common.Hashing;
using Manager.Core.CommonTestsCore;
using Manager.Core.IntegrationTestsCore;
using Manager.RecipientService.Client;
using Manager.RecipientService.Client.BusinessObjects.Requests;
using Manager.RecipientService.Server.Dao.Api.Responses;
using Manager.RecipientService.Server.Dao.Repository;
using Manager.RecipientService.Server.Implementation.Domain;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using RecipientAuthorizationStatus = Manager.RecipientService.Client.BusinessObjects.RecipientAuthorizationStatus;

namespace Manager.RecipientService.IntegrationTests;

public class RecipientServiceApiClientTest : IntegrationTestBase
{
    public RecipientServiceApiClientTest()
    {
        Fixture.Customize<RecipientAccountWithPasswordHash>(composer => composer
            .With(x => x.CreatedAtUtc, Fixture.Create<DateTime>().ToUniversalTime)
            .With(x => x.UpdatedAtUtc, Fixture.Create<DateTime>().ToUniversalTime)
        );
    }

    private IRecipientServiceApiClient RecipientServiceApiClient =>
        ServiceProvider.GetRequiredService<IRecipientServiceApiClient>();

    private IRecipientAccountRepository RecipientAccountRepository =>
        ServiceProvider.GetRequiredService<IRecipientAccountRepository>();

    private IHasher Hasher => ServiceProvider.GetRequiredService<IHasher>();

    [Test]
    public async Task TestCreate()
    {
        // Arrange
        var request = Fixture.Build<CreateRecipientAccountRequest>()
            .With(x => x.RecipientTimeUtcOffsetHours, 7)
            .Create();

        // Act
        var httpResult = await RecipientServiceApiClient.CreateRecipientAccountAsync(request);

        // Assert
        await TestContext.Out.WriteLineAsync(httpResult.ResultMessage);
        httpResult.StatusCode.Should().Be(HttpStatusCode.Created);

        var found = await RecipientAccountRepository.FindByLoginAsync(request.Login);
        found.Should().NotBeNull();

        found.Login.Should().BeEquivalentTo(request.Login);
        found.TimeZoneInfo.BaseUtcOffset.Hours.Should().Be(request.RecipientTimeUtcOffsetHours);
        httpResult.EnsureResponse.Should().BeEquivalentTo(
            new RecipientAccountResponse(
                found.Id,
                found.Login,
                AccountState.Inactive,
                StateReason.NewUser,
                found.TimeZoneInfo.BaseUtcOffset
            )
        );
    }

    [Test]
    public async Task TestCreateWithExistingLogin()
    {
        // Arrange
        var request = Fixture.Build<CreateRecipientAccountRequest>()
            .With(x => x.RecipientTimeUtcOffsetHours, 7)
            .Create();
        var existAccount = Fixture.Build<RecipientAccountWithPasswordHash>()
            .With(x => x.Login, request.Login)
            .With(x => x.CreatedAtUtc, Fixture.Create<DateTime>().ToUniversalTime())
            .With(x => x.UpdatedAtUtc, Fixture.Create<DateTime>().ToUniversalTime())
            .Create();
        await RecipientAccountRepository.CreateAsync(existAccount);

        // Act
        var httpResult = await RecipientServiceApiClient.CreateRecipientAccountAsync(request);

        // Assert
        await TestContext.Out.WriteLineAsync(httpResult.ResultMessage);
        httpResult.StatusCode.Should().Be(HttpStatusCode.Conflict);
        httpResult.ResultMessage.Should().Contain("Аккаунт с таким логином уже существует");
    }

    [TestCase(-20)]
    [TestCase(-13)]
    [TestCase(15)]
    [TestCase(20)]
    public async Task TestCreateWithIncorrectTimeOffset(int offset)
    {
        // Arrange
        var request = Fixture.Build<CreateRecipientAccountRequest>()
            .With(x => x.RecipientTimeUtcOffsetHours, offset)
            .Create();

        // Act
        var httpResult = await RecipientServiceApiClient.CreateRecipientAccountAsync(request);

        // Assert
        await TestContext.Out.WriteLineAsync(httpResult.ResultMessage);
        httpResult.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        httpResult.ResultMessage.Should().Contain("Неправильное смещение времени от всемирного времени UTC");
    }

    [Test]
    public async Task TestCreateWithEmptyLogin()
    {
        // Arrange
        var request = Fixture.Build<CreateRecipientAccountRequest>()
            .With(x => x.RecipientTimeUtcOffsetHours, 7)
            .With(x => x.Login, string.Empty)
            .Create();

        // Act
        var httpResult = await RecipientServiceApiClient.CreateRecipientAccountAsync(request);

        // Assert
        await TestContext.Out.WriteLineAsync(httpResult.ResultMessage);
        httpResult.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        httpResult.ResultMessage.Should().Contain("The Login field is required");
    }

    [Test]
    public async Task TestCreateWithEmptyPassword()
    {
        // Arrange
        var request = Fixture.Build<CreateRecipientAccountRequest>()
            .With(x => x.RecipientTimeUtcOffsetHours, 7)
            .With(x => x.Password, string.Empty)
            .Create();

        // Act
        var httpResult = await RecipientServiceApiClient.CreateRecipientAccountAsync(request);

        // Assert
        await TestContext.Out.WriteLineAsync(httpResult.ResultMessage);
        httpResult.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        httpResult.ResultMessage.Should().Contain("The Password field is required");
    }

    [Test]
    public async Task TestGetWhenAccountIsNotExist()
    {
        // Act
        var httpResult = await RecipientServiceApiClient.GetRecipientAccountAsync(Guid.NewGuid());

        // Assert
        await TestContext.Out.WriteLineAsync(httpResult.ResultMessage);
        httpResult.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task TestGetWhenAccountIsExist()
    {
        // Arrange
        var account = Fixture.Create<RecipientAccountWithPasswordHash>();
        await RecipientAccountRepository.CreateAsync(account);

        // Act
        var httpResult = await RecipientServiceApiClient.GetRecipientAccountAsync(account.Id);

        // Assert
        await TestContext.Out.WriteLineAsync(httpResult.ResultMessage);
        httpResult.StatusCode.Should().Be(HttpStatusCode.OK);
        httpResult.EnsureResponse.Should().BeEquivalentTo(
            new RecipientAccountResponse(
                account.Id,
                account.Login,
                account.State.AccountState,
                account.State.StateReason,
                account.TimeZoneInfo.BaseUtcOffset
            )
        );
    }

    [Test]
    public async Task TestDeleteWhenAccountIsNotExist()
    {
        // Act
        var httpResult = await RecipientServiceApiClient.DeleteRecipientAccountAsync(Guid.NewGuid());

        // Assert
        await TestContext.Out.WriteLineAsync(httpResult.ResultMessage);
        httpResult.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task TestDeleteWhenAccountIsExist()
    {
        // Arrange
        var account = Fixture.Create<RecipientAccountWithPasswordHash>();
        await RecipientAccountRepository.CreateAsync(account);

        // Act
        var httpResult = await RecipientServiceApiClient.DeleteRecipientAccountAsync(account.Id);

        // Assert
        await TestContext.Out.WriteLineAsync(httpResult.ResultMessage);
        httpResult.StatusCode.Should().Be(HttpStatusCode.OK);

        var foundAccount = await RecipientAccountRepository.FindAsync(account.Id);
        foundAccount.Should().NotBeNull();
        foundAccount.State.AccountState.Should().Be(AccountState.Deleted);
        foundAccount.State.StateReason.Should().Be(StateReason.DeletedByUserRequest);
    }

    [Test]
    public async Task TestPatchWhenAccountIsNotExist()
    {
        // Arrange
        var patchRecipientAccountRequest = Fixture.Build<PatchRecipientAccountRequest>()
            .With(x => x.NewRecipientTimeUtcOffsetHours, 1)
            .Create();

        // Act
        var httpResult = await RecipientServiceApiClient.UpdateRecipientAccountAsync(patchRecipientAccountRequest);
g
        // Assert
        await TestContext.Out.WriteLineAsync(httpResult.ResultMessage);
        httpResult.StatusCode.Should().Be(HttpStatusCode.NotFound, httpResult.ResultMessage);
    }

    [Test]
    public async Task TestPatchWhenAccountIsExist()
    {
        // Arrange
        var account = Fixture.Create<RecipientAccountWithPasswordHash>();
        var patchRequest = Fixture.Build<PatchRecipientAccountRequest>()
            .With(x => x.RecipientId, account.Id)
            .With(x => x.NewRecipientTimeUtcOffsetHours, Random.Shared.Next(-12, 14))
            .Create();
        await RecipientAccountRepository.CreateAsync(account);

        // Act
        var httpResult = await RecipientServiceApiClient.UpdateRecipientAccountAsync(patchRequest);

        // Assert
        await TestContext.Out.WriteLineAsync(httpResult.ResultMessage);
        httpResult.StatusCode.Should().Be(HttpStatusCode.OK, httpResult.ResultMessage);

        var foundAccount = await RecipientAccountRepository.FindAsync(account.Id);
        foundAccount.Should().NotBeNull();
        foundAccount.Should().BeEquivalentTo(
            account,
            options => options
                .WithDateTimeCloseTo()
                .Excluding(x => x.Login)
                .Excluding(x => x.PasswordHash)
                .Excluding(x => x.TimeZoneInfo)
                .Excluding(x => x.UpdatedAtUtc)
        );
        foundAccount.Login.Should().BeEquivalentTo(patchRequest.NewLogin);
        Hasher.VerifyHashed(foundAccount.PasswordHash, patchRequest.NewPassword!).IsSuccess.Should().BeTrue();
        foundAccount.TimeZoneInfo.BaseUtcOffset.Hours.Should().Be(patchRequest.NewRecipientTimeUtcOffsetHours);
        foundAccount.UpdatedAtUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Test]
    [Ignore("Метод авторизации получателя пока что всегда возвращает успех, убрать игнор когда уберём заглушку")]
    public async Task TestGetRecipientAuthorizationWhenAccountIsNotExist()
    {
        // Arrange
        var recipientAuthorizationRequest = Fixture.Create<RecipientAuthorizationRequest>();

        // Act
        var httpResult = await RecipientServiceApiClient.GetRecipientAuthorizationAsync(recipientAuthorizationRequest);

        // Assert
        await TestContext.Out.WriteLineAsync(httpResult.ResultMessage);
        httpResult.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task TestGetRecipientAuthorizationWhenAccountIsExist()
    {
        // Arrange
        var account = Fixture.Create<RecipientAccountWithPasswordHash>();
        var recipientAuthorizationRequest = Fixture.Build<RecipientAuthorizationRequest>()
            .With(x => x.RecipientId, account.Id)
            .Create();
        await RecipientAccountRepository.CreateAsync(account);

        // Act
        var httpResult = await RecipientServiceApiClient.GetRecipientAuthorizationAsync(recipientAuthorizationRequest);

        // Assert
        await TestContext.Out.WriteLineAsync(httpResult.ResultMessage);
        httpResult.StatusCode.Should().Be(HttpStatusCode.OK, httpResult.ResultMessage);
        var recipientAuthorizationResponse = httpResult.EnsureResponse;
        recipientAuthorizationResponse.Should().NotBeNull();
        recipientAuthorizationResponse.Should().BeEquivalentTo(recipientAuthorizationRequest);
        recipientAuthorizationResponse.RecipientAuthorizationStatus.Should().Be(RecipientAuthorizationStatus.Success);
    }
}