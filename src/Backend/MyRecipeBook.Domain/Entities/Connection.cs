namespace MyRecipeBook.Domain.Entities;

public class Connection : BaseEntitie
{
    public long UserId { get; set; }
    public long ConnectedWithUserId { get; set; } 
    public User ConnectedWithUser { get; set; }
}
