using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Api.Filter.LoggedUser;
using MyRecipeBook.Application.UseCases.Dashboard;

namespace MyRecipeBook.Api.Controllers;

[ServiceFilter(typeof(AuthenticatedUserAttribute))]
public class DashboardController : MyRecipeBookController
{
    [HttpPut]
    [ProducesResponseType(typeof(Communication.Response.ResponseDashboardJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RecoverDashboard(
        [FromServices] IDashboardUseCase useCase,
        [FromBody] Communication.Request.RequestDashboardJson request)
    {
        var result = await useCase.Execute(request);

        if (result.Recipes.Any())
        {
            return Ok(result);
        }

        return NoContent();
    }
}