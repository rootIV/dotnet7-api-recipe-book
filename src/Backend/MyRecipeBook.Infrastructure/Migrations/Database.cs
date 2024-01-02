using Dapper;
using MySqlConnector;

namespace MyRecipeBook.Infrastructure.Migrations;

public static class Database
{
    public static void CreateDatabase(string connectionString, string schemaName)
    {
        using var myConnection = new MySqlConnection(connectionString);

        var parameters = new DynamicParameters();
        parameters.Add("name", schemaName);

        var registrys = myConnection.Query("SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @name", parameters);

        if (!registrys.Any())
            myConnection.Execute($"CREATE DATABASE {schemaName}");
    }
}
