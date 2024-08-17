using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sports_Merchandise_Store.Models
{
    public class F1Merchandise
    {
        [Key]
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public string ItemSize { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("F1_Team")]
        public int F1TeamId { get; set; }
        public virtual Team Team { get; set; }

        [ForeignKey("Driver")]
        public int DriverId { get; set; }
        public virtual Driver Driver { get; set; }
    }

    //DTOs
    public class F1MerchandiseDTO
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public string ItemSize { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string F1TeamName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}