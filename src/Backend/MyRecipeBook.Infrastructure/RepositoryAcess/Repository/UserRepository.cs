using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositorys.User;

namespace MyRecipeBook.Infrastructure.RepositoryAcess.Repository;

public class UserRepository : IUserWriteOnlyRepository, 
    IUserReadOnlyRepository, 
    IUserUpdateOnlyRepository
{
    private readonly MyRecipeBookContext _context;

    public UserRepository(MyRecipeBookContext context)
    {
        _context = context;
    }

    public async Task Add(User user)
    {
        await _context.Users.AddAsync(user);
    }
    public async Task<bool> ExistUserEmail(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email.Equals(email));
    }
    public async Task<User> RecoverUserByEmail(string email)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Email.Equals(email));
    }
    public async Task<User> RecoverUserByEmailPass(string email, string password)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Email.Equals(email) && c.Password.Equals(password));
    }
    public async Task<User> RecoverUserById(long id)
    {
        return await _context.Users.FirstOrDefaultAsync(c => c.Id == id);
    }
    public void Update(User user)
    {
        _context.Users.Update(user);
    }
}
