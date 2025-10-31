using Manager.RecipientService.Server.Dao.Repository.Dbos;
using Manager.RecipientService.Server.Implementation.Domain;

namespace Manager.RecipientService.Server.Dao.Repository.Converters;

public interface IRecipientAccountStateConverter
{
    RecipientAccountStateWithId ToDto(RecipientAccountStateDbo recipientAccountState);
}