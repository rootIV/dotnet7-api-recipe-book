﻿namespace MyRecipeBook.Domain.Entities;

public class User : BaseEntitie
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
}
