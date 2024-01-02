using FluentValidation;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.User;

public class PasswordValidator : AbstractValidator<string>
{
    public PasswordValidator()
    {
        RuleFor(password => password).NotEmpty().WithMessage(ErroMessagesResource.User_Password_Empty);

        When(password => !string.IsNullOrWhiteSpace(password), () =>
        {
            RuleFor(password => password.Length).GreaterThanOrEqualTo(6).WithMessage(ErroMessagesResource.User_Password_MinChar);
        });
    }
}
