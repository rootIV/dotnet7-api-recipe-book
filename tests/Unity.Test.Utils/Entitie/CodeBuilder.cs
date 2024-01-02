using Bogus;
using MyRecipeBook.Domain.Entities;

namespace Unity.Test.Utils.Entitie;

public class CodeBuilder
{
    public static Codes Build(User user)
    {
        return new Faker<Codes>()
            .RuleFor(c => c.Id, _ => user.Id)
            .RuleFor(c => c.UserId, _ => user.Id)
            .RuleFor(c => c.Code, f => Guid.NewGuid().ToString());
    }
}
