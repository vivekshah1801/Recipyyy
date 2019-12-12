using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Recipyyy.Models
{
    public class Comment
    {
        [Key]
        public int commentId { get; set; }
        public string commentText { get; set; }
        public DateTime commentCreationTime { get; set; }

        public Chef chef { get; set; }
        public int chefId { get; set; }

        public Recipe recipe { get; set; }
        public int? recipeId { get; set; }
    }
}