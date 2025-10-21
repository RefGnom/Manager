using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Manager.Core.CommonTestsCore;
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

public class RecipientAccountRepositoryTest : IntegrationTestBase
{
    private IRecipientAccountRepository RecipientAccountRepository =>
        ServiceProvider.GetRequiredService<IRecipientAccountRepository>();

    private IRecipientAccountConverter RecipientAccountConverter =>
        ServiceProvider.GetRequiredService<IRecipientAccountConverter>();

    private IRecipientAccountStateConverter RecipientAccountStateConverter =>
        ServiceProvider.GetRequiredService<IRecipientAccountStateConverter>();

    [Test]
    public async Task TestCreate()
    {
        // Arrange
        var recipientAccount = Fixture.Build<RecipientAccount>()
            .WithUtcDate(x => x.CreatedAtUtc)
            .WithUtcDate(x => x.UpdatedAtUtc)
            .Create();

        // Act
        await RecipientAccountRepository.CreateAsync(recipientAccount);

        // Assert
        var recipientAccountDbo = await DataContext.FindAsync<RecipientAccountDbo, Guid>(recipientAccount.Id);
        var recipientAccountStateDbo =
            await DataContext.ExecuteReadAsync<RecipientAccountStateDbo, RecipientAccountStateDbo?>(query =>
                query.Where(x => x.AccountState == recipientAccount.State.AccountState)
                    .Where(x => x.StateReason == recipientAccount.State.StateReason)
                    .FirstOrDefaultAsync()
            );
        recipientAccountDbo.Should().NotBeNull();
        recipientAccountStateDbo.Should().NotBeNull();

        var foundRecipientAccount = RecipientAccountConverter.ToDto(
            recipientAccountDbo,
            RecipientAccountStateConverter.ToDto(recipientAccountStateDbo)
        );
        foundRecipientAccount.Should().BeEquivalentTo(
            recipientAccount,
            options => options.WithDateTimeCloseTo().Excluding(x => x.UpdatedAtUtc)
        );
        foundRecipientAccount.UpdatedAtUtc.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Test]
    public async Task TestFindWhenAccountIsNotExisting()
    {
        // Act
        var recipientAccount = await RecipientAccountRepository.FindAsync(Guid.NewGuid());

        // Assert
        recipientAccount.Should().BeNull();
    }

    [Test]
    public async Task TestFindWhenAccountExistingButAccountStateNotExistingShouldThrowsException()
    {
        // Arrange
        var recipientAccountDbo = Fixture.Build<RecipientAccountDbo>()
            .WithUtcDate(x => x.CreatedAtUtc)
            .WithUtcDate(x => x.UpdatedAtUtc)
            .Create();
        await DataContext.InsertAsync(recipientAccountDbo);

        // Assert
        await RecipientAccountRepository.Invoking(x => x.FindAsync(recipientAccountDbo.Id))
            .Should()
            .ThrowAsync<EntityNotFoundException>();
    }

    [Test]
    public async Task TestFindWhenAccountExistingAndStateExisting()
    {
        // Arrange
        var recipientAccountStateDbo = Fixture.Create<RecipientAccountStateDbo>();
        var recipientAccountDbo = Fixture.Build<RecipientAccountDbo>()
            .With(x => x.AccountStateId, recipientAccountStateDbo.Id)
            .With(x => x.TimeZoneInfoId, TimeZoneInfo.Local.Id)
            .WithUtcDate(x => x.CreatedAtUtc)
            .WithUtcDate(x => x.UpdatedAtUtc)
            .Create();
        await DataContext.InsertAsync(recipientAccountStateDbo);
        await DataContext.InsertAsync(recipientAccountDbo);

        // Act
        var foundAccount = await RecipientAccountRepository.FindAsync(recipientAccountDbo.Id);

        // Assert
        foundAccount.Should().NotBeNull();
        var expectedAccount = RecipientAccountConverter.ToDto(
            recipientAccountDbo,
            RecipientAccountStateConverter.ToDto(recipientAccountStateDbo)
        );
        foundAccount.Should().BeEquivalentTo(
            expectedAccount,
            options => options.WithDateTimeCloseTo()
        );
    }
}