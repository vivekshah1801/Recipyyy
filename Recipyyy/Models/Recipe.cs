using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Recipyyy.Models
{
    public class Recipe
    {
        [Key]
        public int recipeId { get; set; }
        public string recipeTitle { get; set; }
        public string recipeDescription { get; set; }
        public string recipeIngredients { get; set; }
        public string recipeDirection { get; set; }
        public int recipeServesFor { get; set; }
        public string recipeType { get; set; }
        public string recipeNutritionFacts { get; set; }
        public bool recipeIsPrivate { get; set; }
        public int recipePrepTime { get; set; }
        public int recipeLikes { get; set; }

        public string recipeTags { get; set; }

        public DateTime recipeCreationDate { get; set; }

        public string recipeImagePath { get; set; }
        [NotMapped]
        public HttpPostedFileBase recipeImageFile { get; set; }

        public int chefId { get; set; }
        public Chef chef { get; set; }

        public int cuisineId { get; set; }
        public Cuisine cusine { get; set; }
        

    }
}