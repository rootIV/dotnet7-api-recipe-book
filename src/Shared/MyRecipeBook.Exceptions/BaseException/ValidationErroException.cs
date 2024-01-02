using System.Runtime.Serialization;

namespace MyRecipeBook.Exceptions.BaseException;

[Serializable]
public class ValidationErroException : MyRecipeBookException
{
    public List<string> ErroMessages { get; set; }

    public ValidationErroException(List<string> messages) : base(string.Empty)
    {
        ErroMessages = messages;
    }
}
