using FluentValidation;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.Login.DoLogin;

public class LoginUserValidator : AbstractValidator<RequestLoginJson>
{
    public LoginUserValidator()
    {
        RuleFor(l => l.Email).NotEmpty().WithMessage(ErroMessagesResource.User_Email_Empty);
        RuleFor(c => c.Password).NotEmpty().WithMessage(ErroMessagesResource.User_Password_Empty);

        When(c => !string.IsNullOrWhiteSpace(c.Email), () =>
        {
            RuleFor(c => c.Email).EmailAddress().WithMessage(ErroMessagesResource.User_Email_Invalid);
        });
        When(c => !string.IsNullOrWhiteSpace(c.Password), () =>
        {
            RuleFor(c => c.Password.Length).GreaterThanOrEqualTo(6).WithMessage(ErroMessagesResource.User_Password_MinChar);
        });
    }
}
