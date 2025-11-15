using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Manager.Core.EFCore;
using Manager.Core.IntegrationTestsCore;
using Manager.RecipientService.Server.Dao.Repository;
using Manager.RecipientService.Server.Dao.Repository.Converters;
using Manager.RecipientService.Server.Dao.Repository.Dbos;
using Manager.RecipientService.Server.Implementation.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Manager.RecipientService.IntegrationTests;

public class RecipientAccountStateRepositoryTest : IntegrationTestBase
{
    private IRecipientAccountStateRepository RecipientAccountStateRepository =>
        ServiceProvider.GetRequiredService<IRecipientAccountStateRepository>();

    private IRecipientAccountStateConverter RecipientAccountStateConverter =>
        ServiceProvider.GetRequiredService<IRecipientAccountStateConverter>();

    [Test]
    public async Task TestReadExistingState()
    {
        // Arrange
        var recipientAccountStateDbo = Fixture.Create<RecipientAccountStateDbo>();
        await DataContext.InsertAsync(recipientAccountStateDbo);

        // Act
        var readState = await RecipientAccountStateRepository.ReadAsync(recipientAccountStateDbo.Id);

        // Assert
        readState.Should().BeEquivalentTo(recipientAccountStateDbo);
    }

    [Test]
    public async Task TestReadNotExistingState()
    {
        // Assert
        await RecipientAccountStateRepository
            .Invoking(x => x.ReadAsync(Guid.NewGuid()))
            .Should()
            .ThrowAsync<EntityNotFoundException>();
    }

    [Test]
    public async Task TestFindOrCreate_Create()
    {
        // Arrange
        var recipientAccountState = Fixture.Create<RecipientAccountState>();
        (await DataContext.ExecuteReadAsync<RecipientAccountStateDbo, RecipientAccountStateDbo?>(query =>
            query.Where(x => x.AccountState == recipientAccountState.AccountState)
                .Where(x => x.StateReason == recipientAccountState.StateReason)
                .FirstOrDefaultAsync()
        )).Should().BeNull();

        // Act
        var readState = await RecipientAccountStateRepository.FindOrCreateAsync(recipientAccountState);

        // Assert
        (await DataContext.ExecuteReadAsync<RecipientAccountStateDbo, RecipientAccountStateDbo?>(query =>
            query.Where(x => x.AccountState == recipientAccountState.AccountState)
                .Where(x => x.StateReason == recipientAccountState.StateReason)
                .FirstOrDefaultAsync()
        )).Should().NotBeNull();
        readState.Should().BeEquivalentTo(recipientAccountState);
    }

    [Test]
    public async Task TestFindOrCreate_Find()
    {
        // Arrange
        var recipientAccountStateDbo = Fixture.Create<RecipientAccountStateDbo>();
        await DataContext.InsertAsync(recipientAccountStateDbo);
        var recipientAccountStateWithId = RecipientAccountStateConverter.ToDto(recipientAccountStateDbo);

        // Act
        var readState = await RecipientAccountStateRepository.FindOrCreateAsync(recipientAccountStateWithId);

        // Assert
        readState.Should().BeEquivalentTo(recipientAccountStateWithId);
    }
}