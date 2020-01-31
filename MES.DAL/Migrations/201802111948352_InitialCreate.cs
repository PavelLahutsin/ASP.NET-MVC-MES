namespace MES.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArrivalOfDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DetailId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Count = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Details", t => t.DetailId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.DetailId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Details",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        VendorCode = c.String(),
                        Quantityq = c.Int(nullable: false),
                        GroupProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GroupProducts", t => t.GroupProductId, cascadeDelete: true)
                .Index(t => t.GroupProductId);
            
            CreateTable(
                "dbo.GroupProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StructureOfTheProducts",
                c => new
                    {
                        ProductId = c.Int(nullable: false),
                        DetailId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductId, t.DetailId })
                .ForeignKey("dbo.Details", t => t.DetailId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.DetailId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Assemblies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Password = c.String(),
                        RoleId = c.Int(nullable: false),
                        Image = c.Binary(),
                        MimeType = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Boxings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Quantity = c.Int(nullable: false),
                        BoxingVariant = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.CheckJmts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Count = c.Int(nullable: false),
                        Airtight = c.Int(),
                        CapM = c.Int(),
                        CapN = c.Int(),
                        Housing = c.Int(),
                        Tube = c.Int(),
                        Center = c.Int(),
                        Defect = c.Int(),
                        Other = c.Int(),
                        RepairCu = c.Int(),
                        RepairNi = c.Int(),
                        RepairCentre = c.Int(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.DefectDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DetailId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Count = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Details", t => t.DetailId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.DetailId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Repairs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Quantity = c.Int(nullable: false),
                        RepairsVariant = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Solderings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.ProductStates",
                c => new
                    {
                        ProductId = c.Int(nullable: false),
                        StateProduct = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductId, t.StateProduct })
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Shipments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Quantity = c.Int(nullable: false),
                        BoxingVariant = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Shipments", "UserId", "dbo.Users");
            DropForeignKey("dbo.Shipments", "ProductId", "dbo.Products");
            DropForeignKey("dbo.StructureOfTheProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductStates", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Solderings", "UserId", "dbo.Users");
            DropForeignKey("dbo.Solderings", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Repairs", "UserId", "dbo.Users");
            DropForeignKey("dbo.Repairs", "ProductId", "dbo.Products");
            DropForeignKey("dbo.DefectDetails", "UserId", "dbo.Users");
            DropForeignKey("dbo.DefectDetails", "DetailId", "dbo.Details");
            DropForeignKey("dbo.CheckJmts", "UserId", "dbo.Users");
            DropForeignKey("dbo.CheckJmts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Boxings", "UserId", "dbo.Users");
            DropForeignKey("dbo.Boxings", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Assemblies", "UserId", "dbo.Users");
            DropForeignKey("dbo.ArrivalOfDetails", "UserId", "dbo.Users");
            DropForeignKey("dbo.Assemblies", "ProductId", "dbo.Products");
            DropForeignKey("dbo.StructureOfTheProducts", "DetailId", "dbo.Details");
            DropForeignKey("dbo.Details", "GroupProductId", "dbo.GroupProducts");
            DropForeignKey("dbo.ArrivalOfDetails", "DetailId", "dbo.Details");
            DropIndex("dbo.Shipments", new[] { "ProductId" });
            DropIndex("dbo.Shipments", new[] { "UserId" });
            DropIndex("dbo.ProductStates", new[] { "ProductId" });
            DropIndex("dbo.Solderings", new[] { "ProductId" });
            DropIndex("dbo.Solderings", new[] { "UserId" });
            DropIndex("dbo.Repairs", new[] { "UserId" });
            DropIndex("dbo.Repairs", new[] { "ProductId" });
            DropIndex("dbo.DefectDetails", new[] { "UserId" });
            DropIndex("dbo.DefectDetails", new[] { "DetailId" });
            DropIndex("dbo.CheckJmts", new[] { "UserId" });
            DropIndex("dbo.CheckJmts", new[] { "ProductId" });
            DropIndex("dbo.Boxings", new[] { "UserId" });
            DropIndex("dbo.Boxings", new[] { "ProductId" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.Assemblies", new[] { "ProductId" });
            DropIndex("dbo.Assemblies", new[] { "UserId" });
            DropIndex("dbo.StructureOfTheProducts", new[] { "DetailId" });
            DropIndex("dbo.StructureOfTheProducts", new[] { "ProductId" });
            DropIndex("dbo.Details", new[] { "GroupProductId" });
            DropIndex("dbo.ArrivalOfDetails", new[] { "UserId" });
            DropIndex("dbo.ArrivalOfDetails", new[] { "DetailId" });
            DropTable("dbo.Shipments");
            DropTable("dbo.ProductStates");
            DropTable("dbo.Solderings");
            DropTable("dbo.Roles");
            DropTable("dbo.Repairs");
            DropTable("dbo.DefectDetails");
            DropTable("dbo.CheckJmts");
            DropTable("dbo.Boxings");
            DropTable("dbo.Users");
            DropTable("dbo.Assemblies");
            DropTable("dbo.Products");
            DropTable("dbo.StructureOfTheProducts");
            DropTable("dbo.GroupProducts");
            DropTable("dbo.Details");
            DropTable("dbo.ArrivalOfDetails");
        }
    }
}
