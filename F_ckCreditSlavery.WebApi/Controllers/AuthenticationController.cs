using AutoMapper;
using F_ckCreditSlavery.Entities.DataTransferObjects;
using F_ckCreditSlavery.Entities.Models;
using F_ckCreditSlavery.WebApi.Filters.Action;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace F_ckCreditSlavery.WebApi.Controllers;

[ApiController]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public AuthenticationController(
        IMapper mapper,
        UserManager<User> userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegisterDto userRegisterDto)
    {
        var user = _mapper.Map<User>(userRegisterDto);
        var result = await _userManager.CreateAsync(user, userRegisterDto.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState);
        }

        await _userManager.AddToRolesAsync(user, userRegisterDto.Roles);

        return StatusCode(201);
    }
}