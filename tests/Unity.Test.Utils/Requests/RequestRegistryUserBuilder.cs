using Bogus;
using MyRecipeBook.Communication.Request;

namespace Unity.Test.Utils.Requests;

public class RequestRegistryUserBuilder
{
    public static RequestRegistryUserJson Build(int passwordLength = 6)
    {
        return new Faker<RequestRegistryUserJson>()
            .RuleFor(c => c.Name, f => f.Person.FullName)
            .RuleFor(c => c.Email, f => f.Internet.Email())
            .RuleFor(c => c.Password, f => f.Internet.Password(passwordLength))
            .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber("## ! ####-####").Replace("!", $"{f.Random.Int(min: 1, max: 9)}"));
    }
}
