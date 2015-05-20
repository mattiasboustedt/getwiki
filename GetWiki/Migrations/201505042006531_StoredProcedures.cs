namespace GetWiki.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoredProcedures : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUserLogins");
            CreateStoredProcedure(
                "dbo.Article_Insert",
                p => new
                    {
                        WikiArticleId = p.Int(),
                        Ns = p.Int(),
                        Title = p.String(),
                        TimeFetched = p.DateTime(),
                        CategoryID = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Articles]([WikiArticleId], [Ns], [Title], [TimeFetched], [CategoryID])
                      VALUES (@WikiArticleId, @Ns, @Title, @TimeFetched, @CategoryID)
                      
                      DECLARE @ArticleID int
                      SELECT @ArticleID = [ArticleID]
                      FROM [dbo].[Articles]
                      WHERE @@ROWCOUNT > 0 AND [ArticleID] = scope_identity()
                      
                      SELECT t0.[ArticleID]
                      FROM [dbo].[Articles] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[ArticleID] = @ArticleID"
            );
            
            CreateStoredProcedure(
                "dbo.Article_Update",
                p => new
                    {
                        ArticleID = p.Int(),
                        WikiArticleId = p.Int(),
                        Ns = p.Int(),
                        Title = p.String(),
                        TimeFetched = p.DateTime(),
                        CategoryID = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Articles]
                      SET [WikiArticleId] = @WikiArticleId, [Ns] = @Ns, [Title] = @Title, [TimeFetched] = @TimeFetched, [CategoryID] = @CategoryID
                      WHERE ([ArticleID] = @ArticleID)"
            );
            
            CreateStoredProcedure(
                "dbo.Article_Delete",
                p => new
                    {
                        ArticleID = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Articles]
                      WHERE ([ArticleID] = @ArticleID)"
            );
            
            CreateStoredProcedure(
                "dbo.Category_Insert",
                p => new
                    {
                        CategoryName = p.String(),
                    },
                body:
                    @"INSERT [dbo].[Categories]([CategoryName])
                      VALUES (@CategoryName)
                      
                      DECLARE @CategoryID int
                      SELECT @CategoryID = [CategoryID]
                      FROM [dbo].[Categories]
                      WHERE @@ROWCOUNT > 0 AND [CategoryID] = scope_identity()
                      
                      SELECT t0.[CategoryID]
                      FROM [dbo].[Categories] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[CategoryID] = @CategoryID"
            );
            
            CreateStoredProcedure(
                "dbo.Category_Update",
                p => new
                    {
                        CategoryID = p.Int(),
                        CategoryName = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[Categories]
                      SET [CategoryName] = @CategoryName
                      WHERE ([CategoryID] = @CategoryID)"
            );
            
            CreateStoredProcedure(
                "dbo.Category_Delete",
                p => new
                    {
                        CategoryID = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Categories]
                      WHERE ([CategoryID] = @CategoryID)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Category_Delete");
            DropStoredProcedure("dbo.Category_Update");
            DropStoredProcedure("dbo.Category_Insert");
            DropStoredProcedure("dbo.Article_Delete");
            DropStoredProcedure("dbo.Article_Update");
            DropStoredProcedure("dbo.Article_Insert");
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId });
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId });
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.AspNetUserLogins", "UserId");
            CreateIndex("dbo.AspNetUserClaims", "UserId");
            CreateIndex("dbo.AspNetUsers", "UserName", unique: true, name: "UserNameIndex");
            CreateIndex("dbo.AspNetUserRoles", "RoleId");
            CreateIndex("dbo.AspNetUserRoles", "UserId");
            CreateIndex("dbo.AspNetRoles", "Name", unique: true, name: "RoleNameIndex");
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles", "Id", cascadeDelete: true);
        }
    }
}
