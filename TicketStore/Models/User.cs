using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace TicketStore.Models
{
    public class User
    {
        public enum gender
        {
            male,
            female
        }
        public enum UserType
        {
            Client,
            Admin
        }


        [Required]
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "Please enter the user name")]
        [StringLength(maximumLength: 20)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Please enter your first name")]
        [StringLength(maximumLength: 20)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Please enter your last name")]
        [StringLength(maximumLength: 20)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Please enter a password")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Enter minimum lower case letter, a character (like '@'), upper case letter and a number")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Enter minimum lower case letter, a character (like '@'), upper case letter and a number")]
        [Compare("Password", ErrorMessage = "Passwords does not match")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        [Required(ErrorMessage = "Please enter a valid email adrress")]
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }

        [Required]
        public gender Gender { get; set; } = 0;//0 = male, 1 = female


        [Required]
        public UserType Type { get; set; } = UserType.Client; //0 user, 1 admin

        //public int CartId { get; set; }




        public ICollection<Ticket> Tickets { get; set; }
        public bool IsAdmin { get; set; } = false;
       // public static Stack<int> UserConnectedByID { get; set; }
        



    }
}
