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
        var request = Fixture.Build<CreateRecipientAccountRequest>().With(x => x.RecipientTimeUtcOffsetHours, 7).Create();

        // Act
        var httpResult = await RecipientServiceApiClient.CreateRecipientAccountAsync(request);

        // Assert
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
}