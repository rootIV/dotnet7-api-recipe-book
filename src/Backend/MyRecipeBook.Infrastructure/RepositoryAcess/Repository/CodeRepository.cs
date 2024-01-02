using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositorys.Code;

namespace MyRecipeBook.Infrastructure.RepositoryAcess.Repository;

public class CodeRepository : ICodeWriteOnlyRepository, ICodeReadOnlyRepository
{
    private readonly MyRecipeBookContext _context;

    public CodeRepository(MyRecipeBookContext context)
    {
        _context = context;
    }

    public async Task Registry(Codes code)
    {
        var databaseCode = await _context.Codes.FirstOrDefaultAsync(c => c.UserId == code.UserId);
        if(databaseCode is not null)
        {
            databaseCode.Code = code.Code;
            _context.Codes.Update(databaseCode);
        }
        else
        {
            await _context.Codes.AddAsync(code);
        }
    }
    public async Task<Codes> RecoverEntitieCode(string code)
    {
        return await _context.Codes.AsNoTracking().FirstOrDefaultAsync(c => c.Code == code);
    }

    public async Task Delete(long userId)
    {
        var codes = await _context.Codes.Where(c => c.UserId == userId).ToListAsync();

        if (codes.Any())
        {
            _context.Codes.RemoveRange(codes);
        }
    }
}
