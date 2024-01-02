using MyRecipeBook.Application.Services.Cryptography;

namespace Unity.Test.Utils.Cryptography;

public class EncPasswordBuilder
{
    public static EncPassword Instance()
    {
        return new EncPassword("Da2y14^51:*=");
    }
}
