using FluentMigrator;

namespace MyRecipeBook.Infrastructure.Migrations.Versions;

[Migration((long)VersionsNumber.CreateUserAssociationTable, "Create User Association Table ")]
public class Version0000004 : Migration
{
    public override void Down()
    {
    }
    public override void Up()
    {
        CreateCodeTable();
        CreateConnectionTable();
    }

    private void CreateCodeTable()
    {
        var table = BaseVersion.InsertDefaultColumns(Create.Table("Codes"));

        table
            .WithColumn("Code").AsString(2000).NotNullable()
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_Code_User_Id", "Users", "Id");
    }
    private void CreateConnectionTable()
    {
        var table = BaseVersion.InsertDefaultColumns(Create.Table("Connections"));

        table
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_Connection_User_Id", "Users", "Id")
            .WithColumn("ConnectedWithUserId").AsInt64().NotNullable().ForeignKey("FK_Connection_ConnectedWithUser_Id", "Users", "Id");
    }
}
