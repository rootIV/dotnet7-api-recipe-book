using FluentMigrator;

namespace MyRecipeBook.Infrastructure.Migrations.Versions;

[Migration((long)VersionsNumber.CreateRecipeTable, "Create Recipe Table")]
public class Version0000002 : Migration
{
    public override void Down()
    {

    }
    public override void Up()
    {
        CreateRecipeTable();
        CreateIngredientsTable();
    }

    private void CreateRecipeTable()
    {
        var table = BaseVersion.InsertDefaultColumns(Create.Table("Recipes"));

        table
            .WithColumn("Title").AsString(100).NotNullable()
            .WithColumn("Category").AsInt16().NotNullable()
            .WithColumn("PreparationMethod").AsString(5000).NotNullable()
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_Recipes_User_Id", "Users", "Id");
    }
    private void CreateIngredientsTable()
    {
        var table = BaseVersion.InsertDefaultColumns(Create.Table("Ingredients"));

        table
            .WithColumn("Product").AsString(100).NotNullable()
            .WithColumn("Quantity").AsString().NotNullable()
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_Ingredients_Recipe_Id", "Recipes", "Id").OnDeleteOrUpdate(System.Data.Rule.Cascade);
    }
}
