using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositorys.Recipe;

namespace MyRecipeBook.Infrastructure.RepositoryAcess.Repository;
public class RecipeRepository : IRecipeWriteOnlyRepository, IRecipeReadOnlyRepository, IRecipeUpdateOnlyRepository
{
    private readonly MyRecipeBookContext _context;

    public RecipeRepository(MyRecipeBookContext context)
    {
        _context = context;
    }

    public async Task Registry(Recipe recipe)
    {
        await _context.Recipes.AddAsync(recipe);
    }
    public void Update(Recipe recipe)
    {
        _context.Recipes.Update(recipe);
    }
    public async Task Delete(long recipeId)
    {
        var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == recipeId);

        _context.Recipes.Remove(recipe);
    }
    public async Task<IList<Recipe>> RecoverUserAllRecipes(long userId)
    {
        return await _context.Recipes
            .AsNoTracking()
            .Include(r => r.Ingredients)
            .Where(r => r.UserId == userId)
            .ToListAsync();
    }
    public async Task<IList<Recipe>> RecoverAllRecipesFromConnectedUsers(List<long> userIds)
    {
        return await _context.Recipes
            .AsNoTracking()
            .Include(r => r.Ingredients)
            .Where(r => userIds.Contains(r.UserId))
            .ToListAsync();
    }
    public async Task<int> RecipeQuantity(long userId)
    {
        return await _context.Recipes.CountAsync(r => r.UserId == userId);
    }
    async Task<Recipe> IRecipeReadOnlyRepository.RecoverRecipeById(long recipeId)
    {
        return await _context.Recipes
            .AsNoTracking()
            .Include(r => r.Ingredients)
            .FirstOrDefaultAsync(r => r.Id == recipeId);
    }
    async Task<Recipe> IRecipeUpdateOnlyRepository.RecoverRecipeById(long recipeId)
    {
        return await _context.Recipes
            .Include(r => r.Ingredients)
            .FirstOrDefaultAsync(r => r.Id == recipeId);
    }
}
