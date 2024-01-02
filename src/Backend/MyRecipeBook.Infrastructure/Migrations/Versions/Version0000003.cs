using FluentMigrator;

namespace MyRecipeBook.Infrastructure.Migrations.Versions;

[Migration((long)VersionsNumber.AddingPreparationTime, "Adding Preparation Time in Table")]
public class Version0000003 : Migration
{
    public override void Down()
    {
    }
    public override void Up()
    {
        Alter.Table("recipes").AddColumn("PreparationTime").AsInt32().NotNullable().WithDefaultValue(0);
    }
}
