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
            //If no value is specified, the default language of the SQL Server instance is used.
            const int LANGUAGE = 0; //1033

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
                                    CREATE FULLTEXT INDEX ON  [dbo].[Ad]([AdTitle] Language {0}, [AdContent] Language {0}) KEY INDEX PK_Ad;
                                end
                                else
                                begin
	                                CREATE FULLTEXT CATALOG [FullTextCatalog] WITH ACCENT_SENSITIVITY = OFF AS DEFAULT;
	                                CREATE FULLTEXT INDEX ON  [dbo].[Ad]([AdTitle] Language {0}, [AdContent] Language {0}) KEY INDEX PK_Ad;
                                end
                           ";
            query = string.Format(query, LANGUAGE);
            migrationBuilder.Sql(query, suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string query = @"
	                                DROP FULLTEXT INDEX ON [dbo].[Ad]; 
		                            DROP FULLTEXT CATALOG [FullTextCatalog]; 
                           ";
            migrationBuilder.Sql(query, suppressTransaction: true);
        }
    }
}
