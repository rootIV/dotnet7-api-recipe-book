using Bogus;
using MyRecipeBook.Domain.Entities;
using Unity.Test.Utils.Cryptography;

namespace Unity.Test.Utils.Entitie;

public class ConnectionBuilder
{
    public static List<User> Build()
    {
        var userConnection = BuildUser();
        userConnection.Id = 4;

        return new List<User>
        {
            userConnection
        };
    }
    public static User BuildUser()
    {
        var userGenerated = new Faker<User>()
            .RuleFor(c => c.Name, f => f.Person.FullName)
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.Password, f =>
            {
                var pass = f.Internet.Password();
                return EncPasswordBuilder.Instance().Encrypt(pass);
            })
            .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber("## ! ####-####").Replace("!", $"{f.Random.Int(min: 1, max: 9)}"));

        return userGenerated;
    }
}
