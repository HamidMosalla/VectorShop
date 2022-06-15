namespace VectorShop.Migrations.IdentityDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdentitySchemaChange : DbMigration
    {
        public override void Up()
        {
            MoveTable(name: "dbo.AspNetRoles", newSchema: "IdentityDb");
            MoveTable(name: "dbo.AspNetUserRoles", newSchema: "IdentityDb");
            MoveTable(name: "dbo.AspNetUsers", newSchema: "IdentityDb");
            MoveTable(name: "dbo.AspNetUserClaims", newSchema: "IdentityDb");
            MoveTable(name: "dbo.AspNetUserLogins", newSchema: "IdentityDb");
        }
        
        public override void Down()
        {
            MoveTable(name: "IdentityDb.AspNetUserLogins", newSchema: "dbo");
            MoveTable(name: "IdentityDb.AspNetUserClaims", newSchema: "dbo");
            MoveTable(name: "IdentityDb.AspNetUsers", newSchema: "dbo");
            MoveTable(name: "IdentityDb.AspNetUserRoles", newSchema: "dbo");
            MoveTable(name: "IdentityDb.AspNetRoles", newSchema: "dbo");
        }
    }
}
