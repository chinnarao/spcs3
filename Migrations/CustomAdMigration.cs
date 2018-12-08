using DbContexts.Ad;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Migrations.AdMigrations
{
    [DbContext(typeof(AdDbContext))]
    [Migration("CustomAdMigration")]
    public class CustomAdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string query = @"
                                IF EXISTS (select * from sys.fulltext_indexes fti WHERE fti.object_id = OBJECT_ID(N'[dbo].[Ad]')) 
                                begin
	                                DROP FULLTEXT INDEX ON [dbo].[Ad]; 
	                                IF EXISTS (select * FROM sys.fulltext_catalogs ftc WHERE ftc.name = N'FullTextCatalog') 
	                                begin
		                                DROP FULLTEXT CATALOG [FullTextCatalog]; 
		                                CREATE FULLTEXT CATALOG [FullTextCatalog] WITH ACCENT_SENSITIVITY = OFF AS DEFAULT;
	                                end
	                                else
	                                begin
		                                CREATE FULLTEXT CATALOG [FullTextCatalog] WITH ACCENT_SENSITIVITY = OFF AS DEFAULT;
	                                end
                                    CREATE FULLTEXT INDEX ON  [dbo].[Ad]([AdTitle]  LANGUAGE 0) KEY INDEX PK_Ad;
                                end
                                else
                                begin
	                                CREATE FULLTEXT CATALOG [FullTextCatalog] WITH ACCENT_SENSITIVITY = OFF AS DEFAULT;
	                                CREATE FULLTEXT INDEX ON  [dbo].[Ad]([AdTitle]  LANGUAGE 0) KEY INDEX PK_Ad;
                                end
                           ";
            migrationBuilder.Sql(query, suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string query = @"
	                                DROP FULLTEXT INDEX ON [dbo].[Article]; 
		                            DROP FULLTEXT CATALOG [FullTextCatalog]; 
                           ";
            migrationBuilder.Sql(query, suppressTransaction: true);
        }
    }
}
