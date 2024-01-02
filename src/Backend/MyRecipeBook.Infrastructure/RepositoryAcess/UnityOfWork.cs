using MyRecipeBook.Domain.Repositorys;

namespace MyRecipeBook.Infrastructure.RepositoryAcess;

public sealed class UnityOfWork : IDisposable, IUnityOfWork
{
    private readonly MyRecipeBookContext _context;
    private bool _disposed;

    public UnityOfWork(MyRecipeBookContext context)
    {
        _context = context;
    }

    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed && disposing)
            _context.Dispose();

        _disposed = true;
    }
}

