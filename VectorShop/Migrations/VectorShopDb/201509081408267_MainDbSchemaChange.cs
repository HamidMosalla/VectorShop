namespace VectorShop.Migrations.VectorShopDb
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MainDbSchemaChange : DbMigration
    {
        public override void Up()
        {
            MoveTable(name: "dbo.Adverts", newSchema: "MainDb");
            MoveTable(name: "dbo.Articles", newSchema: "MainDb");
            MoveTable(name: "dbo.Contacts", newSchema: "MainDb");
            MoveTable(name: "dbo.ControlPanels", newSchema: "MainDb");
            MoveTable(name: "dbo.Links", newSchema: "MainDb");
            MoveTable(name: "dbo.NewDesignOrders", newSchema: "MainDb");
            MoveTable(name: "dbo.NewsLetters", newSchema: "MainDb");
            MoveTable(name: "dbo.PriCats", newSchema: "MainDb");
            MoveTable(name: "dbo.Orders", newSchema: "MainDb");
            MoveTable(name: "dbo.Products", newSchema: "MainDb");
            MoveTable(name: "dbo.SecCats", newSchema: "MainDb");
            MoveTable(name: "dbo.SlideShows", newSchema: "MainDb");
        }
        
        public override void Down()
        {
            MoveTable(name: "MainDb.SlideShows", newSchema: "dbo");
            MoveTable(name: "MainDb.SecCats", newSchema: "dbo");
            MoveTable(name: "MainDb.Products", newSchema: "dbo");
            MoveTable(name: "MainDb.Orders", newSchema: "dbo");
            MoveTable(name: "MainDb.PriCats", newSchema: "dbo");
            MoveTable(name: "MainDb.NewsLetters", newSchema: "dbo");
            MoveTable(name: "MainDb.NewDesignOrders", newSchema: "dbo");
            MoveTable(name: "MainDb.Links", newSchema: "dbo");
            MoveTable(name: "MainDb.ControlPanels", newSchema: "dbo");
            MoveTable(name: "MainDb.Contacts", newSchema: "dbo");
            MoveTable(name: "MainDb.Articles", newSchema: "dbo");
            MoveTable(name: "MainDb.Adverts", newSchema: "dbo");
        }
    }
}
