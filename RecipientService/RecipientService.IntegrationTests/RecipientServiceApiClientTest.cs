using System;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Manager.Core.IntegrationTestsCore;
using Manager.RecipientService.Client;
using Manager.RecipientService.Client.BusinessObjects.Requests;
using Manager.RecipientService.Server.Dao.Api.Responses;
using Manager.RecipientService.Server.Dao.Repository;
using Manager.RecipientService.Server.Implementation.Domain;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Manager.RecipientService.IntegrationTests;

public class RecipientServiceApiClientTest : IntegrationTestBase
{
    private IRecipientServiceApiClient RecipientServiceApiClient =>
        ServiceProvider.GetRequiredService<IRecipientServiceApiClient>();

    private IRecipientAccountRepository RecipientAccountRepository =>
        ServiceProvider.GetRequiredService<IRecipientAccountRepository>();

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
}