using System.ComponentModel.DataAnnotations;

namespace Cards.Auth.Api.Models.ValidationModels
{
    public class Login
    {
        [Required]
        [MaxLength(20)]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
