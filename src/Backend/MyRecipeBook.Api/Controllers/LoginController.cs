using Microsoft.AspNetCore.Mvc;
using MyRecipeBook.Application.UseCases.Login.DoLogin;

namespace MyRecipeBook.Api.Controllers;

public class LoginController : MyRecipeBookController
{
    [HttpPost]
    [ProducesResponseType(typeof(Communication.Response.ResponseLoginJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login(
        [FromServices] ILoginUserUseCase loginUseCase,
        [FromBody] Communication.Request.RequestLoginJson loginRequest)
    {
        var response = await loginUseCase.Execute(loginRequest);

        return Ok(response);
    }
}
