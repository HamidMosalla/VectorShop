namespace VectorShop.Migrations.VectorShopDb
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Adverts",
                c => new
                    {
                        AdvertId = c.Int(nullable: false, identity: true),
                        AdvertDateAdded = c.DateTime(nullable: false),
                        AdvertTitle = c.String(nullable: false),
                        AdvertPicture = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.AdvertId);
            
            CreateTable(
                "dbo.ArticleComments",
                c => new
                    {
                        ArticleCommentId = c.Int(nullable: false, identity: true),
                        ArticleCommentDate = c.DateTime(nullable: false),
                        ArticleCommentEmail = c.String(nullable: false),
                        ArticleCommentName = c.String(nullable: false),
                        ArticleCommentWebsite = c.String(),
                        ArticleCommentBody = c.String(nullable: false),
                        ArticleIDfk = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ArticleCommentId)
                .ForeignKey("dbo.Articles", t => t.ArticleIDfk, cascadeDelete: true)
                .Index(t => t.ArticleIDfk);
            
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        ArticleId = c.Int(nullable: false, identity: true),
                        ArticleTitle = c.String(nullable: false),
                        ArticleDate = c.DateTime(nullable: false),
                        ArticleSummary = c.String(),
                        ArticlePicture = c.String(),
                        ArticleBody = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ArticleId);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        ContactId = c.Int(nullable: false, identity: true),
                        ContactDate = c.DateTime(nullable: false),
                        ContactName = c.String(nullable: false),
                        ContactEmail = c.String(nullable: false),
                        ContactPhone = c.String(),
                        ContactBody = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ContactId);
            
            CreateTable(
                "dbo.Links",
                c => new
                    {
                        LinkId = c.Int(nullable: false, identity: true),
                        LinkPriority = c.Int(nullable: false),
                        LinkTitle = c.String(nullable: false),
                        LinkUrl = c.String(nullable: false),
                        LinkDesc = c.String(),
                    })
                .PrimaryKey(t => t.LinkId);
            
            CreateTable(
                "dbo.NewDesignOrders",
                c => new
                    {
                        NewDesignOrderId = c.Int(nullable: false, identity: true),
                        NewDesignOrderTitle = c.String(nullable: false),
                        NewDesignOrderDate = c.DateTime(nullable: false),
                        NewDesignOrderName = c.String(nullable: false),
                        NewDesignOrderEmail = c.String(nullable: false),
                        NewDesignOrderWebSite = c.String(),
                        NewDesignOrderAddress = c.String(),
                        NewDesignOrderDescBody = c.String(nullable: false),
                        NewDesignOrderPhone = c.String(nullable: false),
                        NewDesignOrderMaxAffordablePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NewDesignOrderHowFindUs = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.NewDesignOrderId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        OrderTitle = c.String(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        OrderFirstName = c.String(nullable: false),
                        OrderLastName = c.String(nullable: false),
                        OrderPhoneNumber = c.String(),
                        OrderEmail = c.String(nullable: false),
                        OrderIsSuccessful = c.Boolean(nullable: false),
                        OrderPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProductIDfk = c.Int(nullable: false),
                        PriCatIDfk = c.Int(nullable: false),
                        SecCatIDfk = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.PriCats", t => t.PriCatIDfk)
                .ForeignKey("dbo.Products", t => t.ProductIDfk)
                .ForeignKey("dbo.SecCats", t => t.SecCatIDfk)
                .Index(t => t.ProductIDfk)
                .Index(t => t.PriCatIDfk)
                .Index(t => t.SecCatIDfk);
            
            CreateTable(
                "dbo.PriCats",
                c => new
                    {
                        PriCatId = c.Int(nullable: false, identity: true),
                        PriCatTitle = c.String(nullable: false),
                        PriCatDesc = c.String(),
                    })
                .PrimaryKey(t => t.PriCatId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        ProductName = c.String(nullable: false),
                        ProductPicture = c.String(nullable: false),
                        ProductDescription = c.String(nullable: false),
                        IsProductFree = c.Boolean(nullable: false),
                        ProductDownloadLink = c.String(nullable: false),
                        ProductDate = c.DateTime(nullable: false),
                        ProductPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriCatIDfk = c.Int(nullable: false),
                        SecCatIDfk = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.PriCats", t => t.PriCatIDfk)
                .ForeignKey("dbo.SecCats", t => t.SecCatIDfk)
                .Index(t => t.PriCatIDfk)
                .Index(t => t.SecCatIDfk);
            
            CreateTable(
                "dbo.SecCats",
                c => new
                    {
                        SecCatId = c.Int(nullable: false, identity: true),
                        SecCatTitle = c.String(nullable: false),
                        SecCatDesc = c.String(),
                        PriCatIDfk = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SecCatId)
                .ForeignKey("dbo.PriCats", t => t.PriCatIDfk, cascadeDelete: true)
                .Index(t => t.PriCatIDfk);
            
            CreateTable(
                "dbo.SlideShows",
                c => new
                    {
                        SlideShowId = c.Int(nullable: false, identity: true),
                        SlideShowTitle = c.String(nullable: false),
                        SlideShowPictrure = c.String(nullable: false),
                        SlideShowLink = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.SlideShowId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "SecCatIDfk", "dbo.SecCats");
            DropForeignKey("dbo.Orders", "ProductIDfk", "dbo.Products");
            DropForeignKey("dbo.Orders", "PriCatIDfk", "dbo.PriCats");
            DropForeignKey("dbo.Products", "SecCatIDfk", "dbo.SecCats");
            DropForeignKey("dbo.SecCats", "PriCatIDfk", "dbo.PriCats");
            DropForeignKey("dbo.Products", "PriCatIDfk", "dbo.PriCats");
            DropForeignKey("dbo.ArticleComments", "ArticleIDfk", "dbo.Articles");
            DropIndex("dbo.SecCats", new[] { "PriCatIDfk" });
            DropIndex("dbo.Products", new[] { "SecCatIDfk" });
            DropIndex("dbo.Products", new[] { "PriCatIDfk" });
            DropIndex("dbo.Orders", new[] { "SecCatIDfk" });
            DropIndex("dbo.Orders", new[] { "PriCatIDfk" });
            DropIndex("dbo.Orders", new[] { "ProductIDfk" });
            DropIndex("dbo.ArticleComments", new[] { "ArticleIDfk" });
            DropTable("dbo.SlideShows");
            DropTable("dbo.SecCats");
            DropTable("dbo.Products");
            DropTable("dbo.PriCats");
            DropTable("dbo.Orders");
            DropTable("dbo.NewDesignOrders");
            DropTable("dbo.Links");
            DropTable("dbo.Contacts");
            DropTable("dbo.Articles");
            DropTable("dbo.ArticleComments");
            DropTable("dbo.Adverts");
        }
    }
}
