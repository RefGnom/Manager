using Manager.RecipientService.Server.Implementation.Domain;

namespace Manager.RecipientService.Server.Implementation.Factories;

public interface IRecipientAccountStateFactory
{
    RecipientAccountState CreateInactiveByNewUser();
    RecipientAccountState CreateDeleted();
}

public class RecipientAccountStateFactory : IRecipientAccountStateFactory
{
    public RecipientAccountState CreateInactiveByNewUser() => new(AccountState.Inactive, StateReason.NewUser);
    public RecipientAccountState CreateDeleted() => new(AccountState.Deleted, StateReason.DeletedByUserRequest);
}