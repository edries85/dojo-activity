using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltExam.Models
{
    public class DojoActivity
    {
        [Key]
        public int DojoActivityId {get;set;}

        [Required(ErrorMessage="Activity name is required!")]
        public string ActivityName {get;set;}

        [Required(ErrorMessage="Date and Time is required")]
        
        public DateTime ActivityDate {get;set;}

        [Required(ErrorMessage="Activity duration is required!")]
        public string Duration{get;set;}
        
        [Required(ErrorMessage= "Unit of time is required")]
        public string DurationUnit{get;set;}

        [Required]
        public string Description{get;set;}
        
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public List<RSVP> Guest {get;set;}
        public int UserId{get;set;}
        public User Creator {get;set;}
        
        
    }
}