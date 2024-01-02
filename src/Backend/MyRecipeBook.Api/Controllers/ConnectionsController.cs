using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Api.Binder;
using MyRecipeBook.Api.Filter.LoggedUser;
using MyRecipeBook.Application.UseCases.Connection.Recover;
using MyRecipeBook.Application.UseCases.Connection.Remove;

namespace MyRecipeBook.Api.Controllers;

[ServiceFilter(typeof(AuthenticatedUserAttribute))]
public class ConnectionsController : MyRecipeBookController
{
    [HttpGet]
    [ProducesResponseType(typeof(Communication.Response.ResponseUserConnectionsJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RecoverConnections([FromServices] IRecoverAllConnectionsUseCase useCase)
    {
        var result = await useCase.Execute();

        if (result.Users.Any())
        {
            return Ok(result);
        }

        return NoContent();
    }
    [HttpDelete]
    [Route("{id:hashids}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveConnection([FromServices] IRemoveConnectionsUseCase useCase,
        [FromRoute][ModelBinder(typeof(HashidsModelBinder))] long id)
    {
        await useCase.Execute(id);

        return NoContent();
    }
}