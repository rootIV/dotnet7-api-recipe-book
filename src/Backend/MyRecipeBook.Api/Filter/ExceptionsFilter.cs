using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.BaseException;
using System.Net;

namespace MyRecipeBook.Api.Filter;

public class ExceptionsFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is MyRecipeBookException)
            TreatMyRecipeBookException(context);
        else
            ThrowUnknowErro(context);
    }

    private static void TreatMyRecipeBookException(ExceptionContext context)
    {
        if (context.Exception is ValidationErroException)
            TreatValidationErroException(context);
        else if (context.Exception is InvalidLoginException)
            TreatLoginException(context);
    }
    private static void TreatValidationErroException(ExceptionContext context)
    {
        var ValidationErrorException = context.Exception as ValidationErroException;

        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Result = new ObjectResult(new ResponseErroJson(ValidationErrorException.ErroMessages));
    }
    private static void TreatLoginException(ExceptionContext context)
    {
        var InvalidLoginException = context.Exception as InvalidLoginException;

        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        context.Result = new ObjectResult(new ResponseErroJson(InvalidLoginException.Message));
    }
    private static void ThrowUnknowErro(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Result = new ObjectResult(new ResponseErroJson(ErroMessagesResource.Unknown_Error));
    }
}
