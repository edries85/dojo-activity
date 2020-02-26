using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BeltExam.Models
{
    public class RSVP
    {
        [Key]
        public int RSVPId {get;set;}
        public int DojoActivityId{get;set;}
        public int UserId {get;set;}
        public User User {get;set;}
        public DojoActivity DojoActivity {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
}