using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sports_Merchandise_Store.Models
{
    public class F1_Team
    {
        [Key]
        public int F1TeamId { get; set; }
        public string F1TeamName { get; set; }
        public string EngineSupplier { get; set; }
        public string Country { get; set; }
        public string F1_TeamImageUrl { get; set; }
    }

    public class F1_TeamDTO
    {
        public int F1TeamId { get; set; }
        public string F1TeamName { get; set; }
        public string EngineSupplier { get; set; }
        public string Country { get; set; }
        public string F1_TeamImageUrl { get; set; }
    }
}
