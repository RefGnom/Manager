namespace Manager.AuthenticationService.Server.Layers.BusinessLogic.Models;

public enum AuthorizationModelState
{
    Unknown = 0,
    Active = 1,
    Expired = 2,
    Revoked = 3,
    Deleted = 4,
}