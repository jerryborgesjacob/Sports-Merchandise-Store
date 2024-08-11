using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sports_Merchandise_Store.Models
{   
    // model for holding data of the soccer merch table
    public class SoccerMerchandise
    {
        [Key]
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public string ItemSize { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("Team")]
        public int Teamid { get; set; }
        public virtual Team Team { get; set; }
        [ForeignKey("Player")]
        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }
    }

    //DTO section
    public class SoccerMerchandiseDTO
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public string ItemSize { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string TeamName { get; set; }
        public string PlayerName { get; set; }
    }
}