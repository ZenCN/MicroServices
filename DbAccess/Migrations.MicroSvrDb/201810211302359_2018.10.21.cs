namespace DbAccess.Migrations.MicroSvrDb
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20181021 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VehicleInStock",
                c => new
                    {
                        SalesID = c.String(nullable: false, maxLength: 128),
                        SalesName = c.String(),
                        HP_ModelID = c.String(),
                        HP_VIN = c.String(),
                        HP_ColorName = c.String(),
                        HP_DeliveryTime = c.String(),
                        SalesStatus = c.String(),
                        HP_DealerOrderStatus = c.String(),
                        ReceiptDateConfirmed = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SalesID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.VehicleInStock");
        }
    }
}
