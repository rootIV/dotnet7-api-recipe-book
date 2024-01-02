using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Api.Binder;
using MyRecipeBook.Api.Filter.LoggedUser;
using MyRecipeBook.Application.UseCases.Recipe.Delete;
using MyRecipeBook.Application.UseCases.Recipe.RecoverById;
using MyRecipeBook.Application.UseCases.Recipe.Registry;
using MyRecipeBook.Application.UseCases.Recipe.Update;

namespace MyRecipeBook.Api.Controllers;

[ServiceFilter(typeof(AuthenticatedUserAttribute))]
public class RecipeController : MyRecipeBookController
{
    [HttpPost]
    [ProducesResponseType(typeof(Communication.Response.ResponseRecipeJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Registry(
        [FromServices] IRegistryRecipeUseCase useCase,
        [FromBody] Communication.Request.RequestRecipeJson request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }
    [HttpGet]
    [Route("{id:hashids}")]
    [ProducesResponseType(typeof(Communication.Response.ResponseRecipeJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecoverById(
        [FromServices] IRecoverRecipeByIdUseCase useCase,
        [FromRoute] [ModelBinder(typeof(HashidsModelBinder))] long id)
    {
        var response = await useCase.Execute(id);

        return Ok(response);
    }
    [HttpPut]
    [Route("{id:hashids}")]
    [ProducesResponseType(typeof(Communication.Response.ResponseRecipeJson), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(
        [FromServices] IUpdateRecipeUseCase useCase,
        [FromBody] Communication.Request.RequestRecipeJson request,
        [FromRoute][ModelBinder(typeof(HashidsModelBinder))] long id)
    {
        await useCase.Execute(id, request);

        return NoContent();
    }
    [HttpDelete]
    [Route("{id:hashids}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(
        [FromServices] IDeleteRecipeUseCase useCase,
        [FromRoute][ModelBinder(typeof(HashidsModelBinder))] long id)
    {
        await useCase.Execute(id);

        return NoContent();
    }
}
