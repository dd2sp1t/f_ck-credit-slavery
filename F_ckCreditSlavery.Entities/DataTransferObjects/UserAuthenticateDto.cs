using System.ComponentModel.DataAnnotations;

namespace F_ckCreditSlavery.Entities.DataTransferObjects;

public class UserAuthenticateDto
{
    [Required(ErrorMessage = $"{nameof(UserName)} is required")]
    public string UserName { get; set; }
    
    [Required(ErrorMessage = $"{nameof(Password)} name is required")]
    public string Password { get; set; }
}