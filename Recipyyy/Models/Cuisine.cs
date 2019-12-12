using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Recipyyy.Models
{
    public class Cuisine
    {
        [Key]
        public int cuisineId { get; set; }
        public string cuisineName { get; set; }

        public string cuisineDescription { get; set; }

        public string cuisineImagePath { get; set; }
        [NotMapped]
        public HttpPostedFileBase cuisineImageFile { get; set; }

        public ICollection<Recipe> recipes { get; set; }
    }
}