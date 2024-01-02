using Microsoft.Extensions.Configuration;

namespace MyRecipeBook.Domain.Extension;

public static class RepositoryExtension
{
    public static string GetDatabaseName(this IConfiguration configurationManager )
    {
        var databaseName = configurationManager.GetConnectionString("DatabaseName");

        return databaseName;
    }

    public static string GetDatabaseConnectionString(this IConfiguration configurationManager)
    {
        var connection = configurationManager.GetConnectionString("Connection");

        return connection;
    }

    public static string GetFullConnection(this IConfiguration configurationManager)
    {
        var databaseName = configurationManager.GetDatabaseName();
        var connection = configurationManager.GetDatabaseConnectionString();

        return $"{connection}Database={databaseName}";
    }
}
