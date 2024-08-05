using System.ComponentModel.DataAnnotations;

namespace Company_Project.DTO
{
    public class RegisterUserDTO
    {

        public string UserName { get; set; }
        public string Password { get; set; }
        [Compare("Password",ErrorMessage ="Password not match !!")]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }

    }
}
