namespace ApiMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigrate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Calls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActionId = c.String(),
                        Type = c.String(),
                        Extend = c.String(),
                        Phone = c.String(),
                        State = c.String(),
                        CallId = c.String(),
                        Channel = c.String(),
                        ChannelStateDesc = c.String(),
                        CallerIdNum = c.String(),
                        CallerIdName = c.String(),
                        ConnectedLineNum = c.String(),
                        ConnectedLineName = c.String(),
                        Exten = c.String(),
                        UniqueId = c.String(),
                        LinkedId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CallHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CallDate = c.DateTime(nullable: false),
                        CallId = c.String(),
                        Recording = c.String(),
                        Play = c.String(),
                        Eplay = c.String(),
                        Download = c.String(),
                        Did = c.String(),
                        Src = c.String(),
                        Dst = c.String(),
                        Status = c.String(),
                        Note = c.String(),
                        Disposition = c.String(),
                        LastApp = c.String(),
                        BillSec = c.Int(nullable: false),
                        Duration = c.Int(nullable: false),
                        Type = c.String(),
                        Duration_Minutes = c.Int(nullable: false),
                        Duration_Seconds = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CallHistories");
            DropTable("dbo.Calls");
        }
    }
}
