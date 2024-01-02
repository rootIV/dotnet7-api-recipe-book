using FluentValidation;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Exceptions;
using System.Text.RegularExpressions;

namespace MyRecipeBook.Application.UseCases.User.Registry;

public class RegistryUserValidator : AbstractValidator<RequestRegistryUserJson>
{
    public RegistryUserValidator()
    {
        RuleFor(c => c.Name).NotEmpty().WithMessage(ErroMessagesResource.User_Name_Empty);
        RuleFor(c => c.Email).NotEmpty().WithMessage(ErroMessagesResource.User_Email_Empty);
        RuleFor(c => c.Phone).NotEmpty().WithMessage(ErroMessagesResource.User_Phone_Empty);
        RuleFor(c => c.Password).SetValidator(new PasswordValidator());

        When(c => !string.IsNullOrWhiteSpace(c.Email), () =>
        {
            RuleFor(c => c.Email).EmailAddress().WithMessage(ErroMessagesResource.User_Email_Invalid);
        });
        When(c => !string.IsNullOrWhiteSpace(c.Phone), () =>
        {
            int timeoutMilliseconds = 5000;
            CancellationTokenSource cancellationTokenSource = new(timeoutMilliseconds);

            RuleFor(c => c.Phone).Custom((phone, context) =>
            {
                string phonePattern = "[0-9]{2} [1-9]{1} [0-9]{4}-[0-9]{4}";
                try
                {
                    var regex = new Regex(phonePattern, RegexOptions.None, TimeSpan.FromMilliseconds(timeoutMilliseconds));
                    var isMatch = regex.IsMatch(phone);

                    if (!isMatch)
                    {
                        context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(phone), ErroMessagesResource.User_Phone_Invalid));
                    }
                }
                catch (RegexMatchTimeoutException)
                {
                    context.AddFailure(new FluentValidation.Results.ValidationFailure(nameof(phone), ErroMessagesResource.Token_Expired));
                }
                finally
                {
                    cancellationTokenSource.Cancel();
                }
            });
        });
    }
}
