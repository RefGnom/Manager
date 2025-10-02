namespace Manager.AuthenticationService.Client.BusinessObjects.Requests;

public record AuthenticationStatusRequest(
    string Service,
    string Resource,
    string ApiKey
);