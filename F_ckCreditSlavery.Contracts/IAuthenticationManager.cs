using F_ckCreditSlavery.Entities.DataTransferObjects;

namespace F_ckCreditSlavery.Contracts;

public interface IAuthenticationManager
{
    Task<bool> ValidateUser(UserAuthenticateDto userForAuth);
    Task<string> CreateToken();
}