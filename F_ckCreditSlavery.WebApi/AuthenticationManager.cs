using F_ckCreditSlavery.Contracts;
using F_ckCreditSlavery.Entities.DataTransferObjects;
using F_ckCreditSlavery.Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace F_ckCreditSlavery.WebApi;

public class AuthenticationManager : IAuthenticationManager
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    
    private User _user;

    public AuthenticationManager(UserManager<User> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public Task<bool> ValidateUser(UserAuthenticateDto userForAuth)
    {
        throw new NotImplementedException();
    }

    public Task<string> CreateToken()
    {
        throw new NotImplementedException();
    }
}