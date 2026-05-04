using System.ComponentModel.DataAnnotations;

namespace Form.Models
{
    public class SignForm
    {
        [Key]

        public int Id { get; set; }


        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mobile is required")]
        [RegularExpression(@"^[6-9]\d{9}$",
        ErrorMessage = "Enter a valid 10-digit mobile number")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$",
        ErrorMessage = "Only valid Gmail address is allowed")]
        public string Email { get; set; }

        [Required(ErrorMessage = "State is required")]
        //public string State { get; set; }
        public int State { get; set; }
        [Required(ErrorMessage = "City is required")]
        //public int City { get; set; }
        public int City { get; set; }

        [Required(ErrorMessage = "Address is required")]
        
        public string Address { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{6,20}$",
        ErrorMessage = "Password must contain uppercase, lowercase, number and special character")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
        public string ConPassword { get; set; }
    }
}
