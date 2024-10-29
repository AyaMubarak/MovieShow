using System.ComponentModel.DataAnnotations;

namespace MovieShow.Models.ViewModels
{
    public class ForgetPassword
    {
        [Required(ErrorMessage = " email is required ")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
