using Manager.RecipientService.Server.Implementation.Domain;

namespace Manager.RecipientService.Server.Implementation.Factories;

public interface IRecipientAccountStateFactory
{
    RecipientAccountState CreateInactiveByNewUser();
}

public class RecipientAccountStateFactory : IRecipientAccountStateFactory
{
    public RecipientAccountState CreateInactiveByNewUser() => new(AccountState.Inactive, StateReason.NewUser);
}