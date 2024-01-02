using Bogus;
using MyRecipeBook.Domain.Entities;
using Unity.Test.Utils.Cryptography;

namespace Unity.Test.Utils.Entitie;

public class UserBuilder
{
    public static (User user, string pass) User1()
    {
        (User user, string pass) = Build();
        user.Id = 1;

        return (user, pass);
    }
    public static (User user, string pass) User2()
    {
        (User user, string pass) = Build();
        user.Id = 2;

        return (user, pass);
    }
    public static (User user, string pass) UserWithConnection()
    {
        (User user, string pass) = Build();
        user.Id = 3;

        return (user, pass);
    }
    public static (User user, string pass) Build()
    {
        string pass = string.Empty;

        var userGenerated = new Faker<User>()
            .RuleFor(c => c.Name, f => f.Person.FullName)
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.Password, f =>
            {
                pass = f.Internet.Password();
                return EncPasswordBuilder.Instance().Encrypt(pass);
            })
            .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber("## ! ####-####").Replace("!", $"{f.Random.Int(min: 1, max: 9)}"));

        return (userGenerated, pass);
    }
}
