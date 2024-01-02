using Bogus;
using MyRecipeBook.Communication.Response;
using Unity.Test.Utils.HashIds;

namespace Unity.Test.Utils.Responses;

public class ResponseUserConnectionBuilder
{
    public static ResponseUserConnectionJson Build()
    {
        var hashIds = HashIdsBuilder.Instance().Build();

        return new Faker<ResponseUserConnectionJson>()
            .RuleFor(c => c.Id, f => hashIds.EncodeLong(f.Random.Long(1, 5000)))
            .RuleFor(c => c.Name, f => f.Person.FullName);
    }
}
