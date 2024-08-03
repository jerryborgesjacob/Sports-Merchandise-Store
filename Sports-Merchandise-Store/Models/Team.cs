using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sports_Merchandise_Store.Models
{
    //Model for holding the team table contents
    public class Team
    {
        
        [Key]
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string League { get; set; }
        public string TeamCountry { get; set; }
        public decimal TeamBudget { get; set; }

        //A team can hold multiple players
        public ICollection<Player> Players { get; set; }
    }

    //DTO section
    public class TeamDTO
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public string League { get; set; }
        public string TeamCountry { get; set; }
        public decimal TeamBudget { get; set; }
    }
}