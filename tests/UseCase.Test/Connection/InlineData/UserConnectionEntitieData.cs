using System.Collections;
using Unity.Test.Utils.Entitie;

namespace UseCase.Test.Connection.InlineData;

public class UserConnectionEntitieData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        var user = ConnectionBuilder.BuildUser();
        var connections = new List<MyRecipeBook.Domain.Entities.User> { user };

        yield return new object[] { user.Id, connections };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
