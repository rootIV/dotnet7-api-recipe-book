namespace MyRecipeBook.Communication.Request;

public class RequestChangePasswordJson
{
    public string ActualPassword { get; set; }
    public string NewPassword { get; set; }
}
