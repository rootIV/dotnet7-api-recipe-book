namespace MyRecipeBook.Domain.Entities;

public class BaseEntitie
{
    public long Id { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}
