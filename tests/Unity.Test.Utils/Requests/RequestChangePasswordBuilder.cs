using Bogus;
using MyRecipeBook.Communication.Request;

namespace Unity.Test.Utils.Requests;

public class RequestChangePasswordBuilder
{
    public static RequestChangePasswordJson Build(int passwordSize = 6)
    {
        return new Faker<RequestChangePasswordJson>()
            .RuleFor(c => c.ActualPassword, f => f.Internet.Password(6))
            .RuleFor(c => c.NewPassword, f => f.Internet.Password(passwordSize));
    }
}
