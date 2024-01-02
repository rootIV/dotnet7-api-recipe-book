using FluentValidation;
using MyRecipeBook.Communication.Request;

namespace MyRecipeBook.Application.UseCases.User.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordJson>
{
    public ChangePasswordValidator()
    {
        RuleFor(c => c.ActualPassword).SetValidator(new PasswordValidator());
        RuleFor(c => c.NewPassword).SetValidator(new PasswordValidator());
    }
}
