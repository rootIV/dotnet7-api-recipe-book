using System.Runtime.Serialization;

namespace MyRecipeBook.Exceptions.BaseException;

[Serializable]
public class InvalidLoginException : MyRecipeBookException
{
    public InvalidLoginException() : base(ErroMessagesResource.User_Login_Invalid) { }
    protected InvalidLoginException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
