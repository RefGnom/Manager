using Manager.Core.Networking;

namespace Manager.RecipientService.Client;

public interface IRecipientServiceApiClientFactory
{
    IRecipientServiceApiClient Create(string apiKey);
}

public class RecipientServiceApiClientFactory(IPortProvider portProvider) : IRecipientServiceApiClientFactory
{
    public IRecipientServiceApiClient Create(string apiKey) =>
        new RecipientServiceApiClient($"http://localhost:{portProvider.GetPort("RECIPIENT_SERVICE_PORT")}", apiKey);
}