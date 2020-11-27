using System.ComponentModel.DataAnnotations;

namespace PruebaAranda.Entitis
{
    public class LoginRequestView
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
