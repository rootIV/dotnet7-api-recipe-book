using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Api.Filter.LoggedUser;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Application.UseCases.User.RecoverProfile;
using MyRecipeBook.Application.UseCases.User.Registry;

namespace MyRecipeBook.Api.Controllers;

public class UserController : MyRecipeBookController
{
    [HttpPost]
    [ProducesResponseType(typeof(Communication.Response.ResponseRegistryUserJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> RegistryUser(
        [FromServices] IRegistryUserUseCase useCase,
        [FromBody] Communication.Request.RequestRegistryUserJson request)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }
    [HttpPut]
    [Route("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ServiceFilter(typeof(AuthenticatedUserAttribute))]
    public async Task<IActionResult> ChangePassword(
        [FromServices] IChangePasswordUseCase useCase,
        [FromBody] Communication.Request.RequestChangePasswordJson request)
    {
        await useCase.Execute(request);

        return NoContent();
    }
    [HttpGet]
    [ProducesResponseType(typeof(Communication.Response.ResponseUserProfileJson), StatusCodes.Status200OK)]
    [ServiceFilter(typeof(AuthenticatedUserAttribute))]
    public async Task<IActionResult> RecoverProfile(
        [FromServices] IRecoverProfileUseCase useCase)
    {
        var response = await useCase.Execute();

        return Ok(response);
    }
}