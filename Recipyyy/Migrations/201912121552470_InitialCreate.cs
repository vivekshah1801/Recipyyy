namespace Recipyyy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Chefs",
                c => new
                    {
                        chefId = c.Int(nullable: false, identity: true),
                        chefName = c.String(),
                        chefUsername = c.String(),
                        chefBio = c.String(),
                        chefRating = c.Int(nullable: false),
                        chefTipsEnable = c.Boolean(nullable: false),
                        chefCreationDate = c.DateTime(nullable: false),
                        chefImagePath = c.String(),
                        Chef_chefId = c.Int(),
                    })
                .PrimaryKey(t => t.chefId)
                .ForeignKey("dbo.Chefs", t => t.Chef_chefId)
                .Index(t => t.Chef_chefId);
            
            CreateTable(
                "dbo.Recipes",
                c => new
                    {
                        recipeId = c.Int(nullable: false, identity: true),
                        recipeTitle = c.String(),
                        recipeDescription = c.String(),
                        recipeIngredients = c.String(),
                        recipeDirection = c.String(),
                        recipeServesFor = c.Int(nullable: false),
                        recipeType = c.String(),
                        recipeNutritionFacts = c.String(),
                        recipeIsPrivate = c.Boolean(nullable: false),
                        recipePrepTime = c.Int(nullable: false),
                        recipeLikes = c.Int(nullable: false),
                        recipeTags = c.String(),
                        recipeCreationDate = c.DateTime(nullable: false),
                        recipeImagePath = c.String(),
                        chefId = c.Int(nullable: false),
                        cuisineId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.recipeId)
                .ForeignKey("dbo.Chefs", t => t.chefId, cascadeDelete: true)
                .ForeignKey("dbo.Cuisines", t => t.cuisineId, cascadeDelete: true)
                .Index(t => t.chefId)
                .Index(t => t.cuisineId);
            
            CreateTable(
                "dbo.Cuisines",
                c => new
                    {
                        cuisineId = c.Int(nullable: false, identity: true),
                        cuisineName = c.String(),
                        cuisineDescription = c.String(),
                        cuisineImagePath = c.String(),
                    })
                .PrimaryKey(t => t.cuisineId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        commentId = c.Int(nullable: false, identity: true),
                        commentText = c.String(),
                        commentCreationTime = c.DateTime(nullable: false),
                        chefId = c.Int(nullable: false),
                        recipeId = c.Int(),
                    })
                .PrimaryKey(t => t.commentId)
                .ForeignKey("dbo.Chefs", t => t.chefId, cascadeDelete: true)
                .ForeignKey("dbo.Recipes", t => t.recipeId)
                .Index(t => t.chefId)
                .Index(t => t.recipeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "recipeId", "dbo.Recipes");
            DropForeignKey("dbo.Comments", "chefId", "dbo.Chefs");
            DropForeignKey("dbo.Recipes", "cuisineId", "dbo.Cuisines");
            DropForeignKey("dbo.Recipes", "chefId", "dbo.Chefs");
            DropForeignKey("dbo.Chefs", "Chef_chefId", "dbo.Chefs");
            DropIndex("dbo.Comments", new[] { "recipeId" });
            DropIndex("dbo.Comments", new[] { "chefId" });
            DropIndex("dbo.Recipes", new[] { "cuisineId" });
            DropIndex("dbo.Recipes", new[] { "chefId" });
            DropIndex("dbo.Chefs", new[] { "Chef_chefId" });
            DropTable("dbo.Comments");
            DropTable("dbo.Cuisines");
            DropTable("dbo.Recipes");
            DropTable("dbo.Chefs");
        }
    }
}
