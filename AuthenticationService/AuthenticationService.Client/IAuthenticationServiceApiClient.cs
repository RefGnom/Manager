using System.Threading.Tasks;
using Manager.AuthenticationService.Client.BusinessObjects.Requests;
using Manager.AuthenticationService.Client.BusinessObjects.Responses;

namespace Manager.AuthenticationService.Client;

public interface IAuthenticationServiceApiClient
{
    Task<AuthenticationStatusResponse> GetAuthenticationStatusAsync(
        AuthenticationStatusRequest authenticationStatusRequest
    );
}