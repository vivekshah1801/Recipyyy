using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Recipyyy.Models
{
    public class Chef
    {
        [Key]
        public int chefId { get; set;}
        public string chefName { get; set; }

        // comes from identity
        public string chefUsername { get; set; }

        public string chefBio { get; set; }
        public int chefRating { get; set; }
        public bool chefTipsEnable { get; set; }
        
        public DateTime chefCreationDate { get; set; }

        public string chefImagePath { get; set; }
        [NotMapped]
        public HttpPostedFileBase chefImageFile { get; set; }

        public ICollection<Recipe> recipes { get; set; }

        //saves the list of followers of current chef. Email these followers when curent user adds a recipe
        public ICollection<Chef> chefFollowerList { get; set; }
    }
}