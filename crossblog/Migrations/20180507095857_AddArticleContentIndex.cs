using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace crossblog.Migrations
{
    public partial class AddArticleContentIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE `crossblog`.`Articles` ADD FULLTEXT INDEX `ContentTextIndex` (`Content` ASC);");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE `crossblog`.`Articles` DROP INDEX `textIndex`;");
        }
    }
}
