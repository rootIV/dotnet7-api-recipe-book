using MyRecipeBook.Communication.Request;
using MyRecipeBook.Communication.Response;

namespace MyRecipeBook.Application.UseCases.Dashboard;

public interface IDashboardUseCase
{
    Task<Communication.Response.ResponseDashboardJson> Execute(Communication.Request.RequestDashboardJson request);
}
