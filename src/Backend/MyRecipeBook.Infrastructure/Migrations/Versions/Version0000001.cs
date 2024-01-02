using FluentMigrator;

namespace MyRecipeBook.Infrastructure.Migrations.Versions;

[Migration((long)VersionsNumber.CreateUserTable, "Create User Table")]
public class Version0000001 : Migration
{
    public override void Down()
    {

    }
    public override void Up()
    {
        var table = BaseVersion.InsertDefaultColumns(Create.Table("Users"));

        table
            .WithColumn("Name").AsString(100).NotNullable()
            .WithColumn("Email").AsString(100).NotNullable()
            .WithColumn("Password").AsString(2000).NotNullable()
            .WithColumn("Phone").AsString().NotNullable();
    }
}
