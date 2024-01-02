using MyRecipeBook.Application.Services.Token;

namespace Unity.Test.Utils.Token;

public class TokenControllerBuilder
{
    public static TokenController Instance()
    {
        return new TokenController(1000, "5nRI93a5AEkZWlOz6Iv76etKnrSyCqX3LIXlxKPHdBA=");
    }
    public static TokenController ExpiredToken()
    {
        return new TokenController(0.01667, "5nRI93a5AEkZWlOz6Iv76etKnrSyCqX3LIXlxKPHdBA=");
    }
}
