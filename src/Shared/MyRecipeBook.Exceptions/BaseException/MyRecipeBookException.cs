using System.Runtime.Serialization;

namespace MyRecipeBook.Exceptions.BaseException;

[Serializable]
public class MyRecipeBookException : SystemException 
{
    public MyRecipeBookException(string message) : base(message) { }
    protected MyRecipeBookException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}