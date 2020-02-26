using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace BeltExam.Models
{
    public class User
    {
        [Key]
        public int UserId {get; set;}

        [Required(ErrorMessage="First name is required!")]
        [MinLength(2, ErrorMessage="First name must be at least 2 characters")]
        public string FirstName {get;set;}

        [Required(ErrorMessage="Last name is required!")]
        [MinLength(2, ErrorMessage="Last name must be at least 2 characters")]
        public string LastName {get; set;}

        [Required(ErrorMessage="Email address is required for registration")]
        [EmailAddress]
        public string Email{get; set;}

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage="Password must be at least 8 characters")]
        [RegularExpression("^(?=.{8,})(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=]).*$", ErrorMessage="Password must contain  1 uppercase letter and 1 special character! ")]
        public string Password {get; set;}

        
        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime UpdatedAt {get; set;} = DateTime.Now;
        public List<RSVP> Events {get;set;}

        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage="Password and confirm must match")]
        public string ConfirmPassword{get;set;}

        

    }
}