using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace VectorShop.Models
{
    public class VectorShopDb : DbContext
    {
        public VectorShopDb()
            : base("VectorShopDb")
        {
            //Database.Log = sql => Debug.Write(sql);
        }
        
        public DbSet<Advert> Adverts { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<NewDesignOrder> NewDesignOrders { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PriCat> PriCats { get; set; }
        public DbSet<SecCat> SecCats { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SlideShow> SlideShows { get; set; }
        public DbSet<ControlPanel> ControlPanels { get; set; }
        public DbSet<NewsLetter> NewsLetters { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Enable-Migrations -ContextTypeName FullyQualifiedContextName -MigrationsDirectory Migrations\YourFolderName
            //Add-Migration -ConfigurationTypeName VectorShop.Migrations.VectorShopDb.Configuration "YourMigrationName"
            //Update-Database -ConfigurationTypeName VectorShop.Migrations.VectorShopDb.Configuration -verbose
            //Update-Database -ConfigurationTypeName VectorShop.Migrations.IdentityDbContext.Configuration -verbose

            modelBuilder.HasDefaultSchema("MainDb");

            #region Stopping Cascading deletes on order and product.
            //it might be redundant, you could do it in setting referential integrities part too, simply
            //by adding .WillCascadeOnDelete(false); and the end, but I won't change it now, 
            //just in case I might fuck things up

            //Stop cascading delete Product=>Order
            modelBuilder.Entity<Order>()
               .HasRequired(t => t.Product)
               .WithMany(t => t.Orders)
               .HasForeignKey(d => d.ProductIDfk)
               .WillCascadeOnDelete(false);

            //Stop cascading delete PriCat=>Order
            modelBuilder.Entity<Order>()
               .HasRequired(t => t.PriCat)
               .WithMany(t => t.Orders)
               .HasForeignKey(d => d.PriCatIDfk)
               .WillCascadeOnDelete(false);

            //Stop cascading delete SecCat=>Order
            modelBuilder.Entity<Order>()
               .HasRequired(t => t.SecCat)
               .WithMany(t => t.Orders)
               .HasForeignKey(d => d.SecCatIDfk)
               .WillCascadeOnDelete(false);

            //Stop cascading delete PriCat=>Product
            modelBuilder.Entity<Product>()
               .HasRequired(t => t.PriCat)
               .WithMany(t => t.Products)
               .HasForeignKey(d => d.PriCatIDfk)
               .WillCascadeOnDelete(false);

            //Stop cascading delete SecCat=>Product
            modelBuilder.Entity<Product>()
               .HasRequired(t => t.SecCat)
               .WithMany(t => t.Products)
               .HasForeignKey(d => d.SecCatIDfk)
               .WillCascadeOnDelete(false);

            //Stop cascading delete PriCat=>NewsLetter
            modelBuilder.Entity<NewsLetter>()
               .HasRequired(t => t.PriCat)
               .WithMany(t => t.NewsLetters)
               .HasForeignKey(d => d.PriCatIDfk)
               .WillCascadeOnDelete(false);

            //Stop cascading delete SecCat=>NewsLetter
            modelBuilder.Entity<NewsLetter>()
               .HasRequired(t => t.SecCat)
               .WithMany(t => t.NewsLetters)
               .HasForeignKey(d => d.SecCatIDfk)
               .WillCascadeOnDelete(false);
            #endregion 

            #region setting referential integrities
            //One Product , many Order
            modelBuilder.Entity<Order>()
                .HasRequired(p => p.Product)
                .WithMany(p => p.Orders)
                .HasForeignKey(p => p.ProductIDfk);
            //****************************************//
            //One PriCat, many SecCat
            modelBuilder.Entity<SecCat>()
                .HasRequired(p => p.PriCat)
                .WithMany(p => p.SecCats)
                .HasForeignKey(p => p.PriCatIDfk);
            //****************************************//

            //One PriCat, many Order
            modelBuilder.Entity<Order>()
                .HasOptional(p => p.PriCat)
                .WithMany(p => p.Orders)
                .HasForeignKey(p => p.PriCatIDfk);

            //One SecCat, many Order
            modelBuilder.Entity<Order>()
                .HasOptional(p => p.SecCat)
                .WithMany(p => p.Orders)
                .HasForeignKey(p => p.SecCatIDfk);
            //****************************************//

            //One PriCat, many Product
            modelBuilder.Entity<Product>()
                .HasOptional(p => p.PriCat)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.PriCatIDfk);

            //One SecCat, many Product
            modelBuilder.Entity<Product>()
                .HasOptional(p => p.SecCat)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.SecCatIDfk);

            //One PriCat, many NewsLetter
            modelBuilder.Entity<NewsLetter>()
                .HasOptional(p => p.PriCat)
                .WithMany(p => p.NewsLetters)
                .HasForeignKey(p => p.PriCatIDfk);

            //One SecCat, many NewsLetter
            modelBuilder.Entity<NewsLetter>()
                .HasOptional(p => p.SecCat)
                .WithMany(p => p.NewsLetters)
                .HasForeignKey(p => p.SecCatIDfk);
            //****************************************//
            #endregion

            #region List of required field in database, timmy? timmy! jimmy, timmy!
            modelBuilder.Entity<Advert>().Property(t => t.AdvertDateAdded).IsRequired();
            modelBuilder.Entity<Advert>().Property(t => t.AdvertPicture).IsRequired();
            modelBuilder.Entity<Advert>().Property(t => t.AdvertTitle).IsRequired();

            modelBuilder.Entity<Article>().Property(t => t.ArticleBody).IsRequired();
            modelBuilder.Entity<Article>().Property(t => t.ArticleDate).IsRequired();
            modelBuilder.Entity<Article>().Property(t => t.ArticleTitle).IsRequired();

            modelBuilder.Entity<Contact>().Property(t => t.ContactBody).IsRequired();
            modelBuilder.Entity<Contact>().Property(t => t.ContactDate).IsRequired();
            modelBuilder.Entity<Contact>().Property(t => t.ContactEmail).IsRequired();
            modelBuilder.Entity<Contact>().Property(t => t.ContactName).IsRequired();

            modelBuilder.Entity<Link>().Property(t => t.LinkPriority).IsRequired();
            modelBuilder.Entity<Link>().Property(t => t.LinkTitle).IsRequired();
            modelBuilder.Entity<Link>().Property(t => t.LinkUrl).IsRequired();

            modelBuilder.Entity<NewDesignOrder>().Property(t => t.NewDesignOrderDescBody).IsRequired();
            modelBuilder.Entity<NewDesignOrder>().Property(t => t.NewDesignOrderEmail).IsRequired();
            modelBuilder.Entity<NewDesignOrder>().Property(t => t.NewDesignOrderHowFindUs).IsRequired();
            modelBuilder.Entity<NewDesignOrder>().Property(t => t.NewDesignOrderMaxAffordablePrice).IsRequired();
            modelBuilder.Entity<NewDesignOrder>().Property(t => t.NewDesignOrderName).IsRequired();
            modelBuilder.Entity<NewDesignOrder>().Property(t => t.NewDesignOrderDate).IsRequired();
            modelBuilder.Entity<NewDesignOrder>().Property(t => t.NewDesignOrderPhone).IsRequired();
            modelBuilder.Entity<NewDesignOrder>().Property(t => t.NewDesignOrderTitle).IsRequired();

            modelBuilder.Entity<Order>().Property(t => t.OrderDate).IsRequired();
            modelBuilder.Entity<Order>().Property(t => t.OrderEmail).IsRequired();
            modelBuilder.Entity<Order>().Property(t => t.OrderFirstName).IsRequired();
            modelBuilder.Entity<Order>().Property(t => t.OrderLastName).IsRequired();
            modelBuilder.Entity<Order>().Property(t => t.OrderPrice).IsRequired();
            modelBuilder.Entity<Order>().Property(t => t.OrderTitle).IsRequired();
            modelBuilder.Entity<Order>().Property(t => t.PriCatIDfk).IsOptional();
            modelBuilder.Entity<Order>().Property(t => t.ProductIDfk).IsRequired();
            modelBuilder.Entity<Order>().Property(t => t.SecCatIDfk).IsOptional();

            modelBuilder.Entity<PriCat>().Property(t => t.PriCatTitle).IsRequired();

            modelBuilder.Entity<SecCat>().Property(t => t.PriCatIDfk).IsRequired();
            modelBuilder.Entity<SecCat>().Property(t => t.SecCatTitle).IsRequired();

            modelBuilder.Entity<Product>().Property(t => t.IsProductFree).IsRequired();
            modelBuilder.Entity<Product>().Property(t => t.PriCatIDfk).IsOptional();
            modelBuilder.Entity<Product>().Property(t => t.ProductDate).IsRequired();
            modelBuilder.Entity<Product>().Property(t => t.ProductDescription).IsRequired();
            modelBuilder.Entity<Product>().Property(t => t.ProductDownloadLink).IsRequired();
            modelBuilder.Entity<Product>().Property(t => t.ProductName).IsRequired();
            modelBuilder.Entity<Product>().Property(t => t.ProductPicture).IsRequired();
            modelBuilder.Entity<Product>().Property(t => t.ProductPrice).IsRequired();
            modelBuilder.Entity<Product>().Property(t => t.SecCatIDfk).IsOptional();

            modelBuilder.Entity<SlideShow>().Property(t => t.SlideShowLink).IsRequired();
            modelBuilder.Entity<SlideShow>().Property(t => t.SlideShowPictrure).IsRequired();
            modelBuilder.Entity<SlideShow>().Property(t => t.SlideShowTitle).IsRequired();
            modelBuilder.Entity<SlideShow>().Property(t => t.SlideShowDescription).IsOptional();

            modelBuilder.Entity<ControlPanel>().Property(t => t.SlideShowNumber).IsRequired();
            modelBuilder.Entity<ControlPanel>().Property(t => t.About).IsRequired();

            modelBuilder.Entity<NewsLetter>().Property(t => t.NewsLetterEmail).IsRequired();
            modelBuilder.Entity<NewsLetter>().Property(t => t.IsActive).IsRequired();
            #endregion

        }

    }
}