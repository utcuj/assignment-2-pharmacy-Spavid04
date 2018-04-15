namespace Pharmacy.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InvoiceContents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Int(nullable: false),
                        Invoice_Id = c.Int(),
                        Medication_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Invoices", t => t.Invoice_Id)
                .ForeignKey("dbo.Medications", t => t.Medication_Id)
                .Index(t => t.Invoice_Id)
                .Index(t => t.Medication_Id);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientIdentifier = c.String(),
                        Date = c.DateTime(nullable: false),
                        Issuer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Issuer_Id)
                .Index(t => t.Issuer_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Name = c.String(),
                        UserType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Medications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Manufacturer = c.String(),
                        Ingredients = c.String(),
                        Price = c.Int(nullable: false),
                        Stock = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InvoiceContents", "Medication_Id", "dbo.Medications");
            DropForeignKey("dbo.InvoiceContents", "Invoice_Id", "dbo.Invoices");
            DropForeignKey("dbo.Invoices", "Issuer_Id", "dbo.Users");
            DropIndex("dbo.Invoices", new[] { "Issuer_Id" });
            DropIndex("dbo.InvoiceContents", new[] { "Medication_Id" });
            DropIndex("dbo.InvoiceContents", new[] { "Invoice_Id" });
            DropTable("dbo.Medications");
            DropTable("dbo.Users");
            DropTable("dbo.Invoices");
            DropTable("dbo.InvoiceContents");
        }
    }
}
