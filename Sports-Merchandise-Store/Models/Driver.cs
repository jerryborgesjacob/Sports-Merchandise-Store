using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sports_Merchandise_Store.Models
{
    public class Driver
    {
        [Key]
        public int DriverId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [ForeignKey("F1_Team")]
        public int F1TeamId { get; set; }
        public virtual F1_Team F1_Team { get; set; }
    }
}