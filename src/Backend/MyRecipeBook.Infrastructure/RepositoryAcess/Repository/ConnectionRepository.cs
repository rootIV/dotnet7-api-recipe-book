using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositorys.Connection;

namespace MyRecipeBook.Infrastructure.RepositoryAcess.Repository;

public class ConnectionRepository : IConnectionReadOnlyRepository, IConnectionWriteOnlyRepository
{
    private readonly MyRecipeBookContext _context;

    public ConnectionRepository(MyRecipeBookContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsConnection(long userIdA, long userIdB)
    {
        return await _context.Connections.AnyAsync(c => c.UserId == userIdA && c.ConnectedWithUserId == userIdB);
    }
    public async Task<IList<User>> RecoverConnectedUsers(long userId)
    {
        return await _context.Connections
            .AsNoTracking()
            .Include(c => c.ConnectedWithUser)
            .Where(c => c.UserId == userId)
            .Select(c => c.ConnectedWithUser)
            .ToListAsync();
    }
    public async Task Registry(Connection connection)
    {
        await _context.Connections.AddAsync(connection);
    }
    public async Task RemoveConnection(long userId, long connectedUserIdToRemove)
    {
        var connections = await _context.Connections
            .Where(c => (c.UserId == userId && c.ConnectedWithUserId == connectedUserIdToRemove) ||
            (c.UserId == connectedUserIdToRemove && c.ConnectedWithUserId == userId))
            .ToListAsync();

        _context.Connections.RemoveRange(connections);
    }
}
