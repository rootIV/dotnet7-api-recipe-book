﻿namespace MyRecipeBook.Domain.Repositorys.User;

public interface IUserUpdateOnlyRepository
{
    void Update(Entities.User user);
    Task<Entities.User> RecoverUserById(long id);
}
