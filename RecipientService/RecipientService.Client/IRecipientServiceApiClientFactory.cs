namespace Manager.RecipientService.Client;

public interface IRecipientServiceApiClientFactory
{
    IRecipientServiceApiClient Create(string port, string apiKey);
}

public class RecipientServiceApiClientFactory : IRecipientServiceApiClientFactory
{
    public IRecipientServiceApiClient Create(string port, string apiKey) =>
        new RecipientServiceApiClient($"http://localhost:{port}/api", apiKey);
}