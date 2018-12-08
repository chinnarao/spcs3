using DbContexts.Article;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Migrations.ArticleMigrations
{
    [DbContext(typeof(ArticleDbContext))]
    [Migration("CustomArticleMigration")]
    public class CustomArticleMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string query = @"
                                IF EXISTS (select * from sys.fulltext_indexes fti WHERE fti.object_id = OBJECT_ID(N'[dbo].[Article]')) 
                                begin
	                                DROP FULLTEXT INDEX ON [dbo].[Article]; 
	                                IF EXISTS (select * FROM sys.fulltext_catalogs ftc WHERE ftc.name = N'FullTextCatalog') 
	                                begin
		                                DROP FULLTEXT CATALOG [FullTextCatalog]; 
		                                CREATE FULLTEXT CATALOG [FullTextCatalog] WITH ACCENT_SENSITIVITY = OFF AS DEFAULT;
	                                end
	                                else
	                                begin
		                                CREATE FULLTEXT CATALOG [FullTextCatalog] WITH ACCENT_SENSITIVITY = OFF AS DEFAULT;
	                                end
                                    CREATE FULLTEXT INDEX ON  [dbo].[Article]([Title]  LANGUAGE 0) KEY INDEX PK_Article;
                                end
                                else
                                begin
	                                CREATE FULLTEXT CATALOG [FullTextCatalog] WITH ACCENT_SENSITIVITY = OFF AS DEFAULT;
	                                CREATE FULLTEXT INDEX ON  [dbo].[Article]([Title]  LANGUAGE 0) KEY INDEX PK_Article;
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
