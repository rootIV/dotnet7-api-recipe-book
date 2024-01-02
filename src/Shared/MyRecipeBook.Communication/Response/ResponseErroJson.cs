namespace MyRecipeBook.Communication.Response;

public class ResponseErroJson
{
    public List<string> ErroMessages { get; set; }

    public ResponseErroJson(string message)
    {
        ErroMessages = new List<string>
        {
            message
        };
    }
    public ResponseErroJson(List<string> messages)
    {
        ErroMessages = messages;
    }
}
