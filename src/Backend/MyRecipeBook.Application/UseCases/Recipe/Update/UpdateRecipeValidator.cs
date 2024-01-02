using FluentValidation;
using MyRecipeBook.Communication.Request;

namespace MyRecipeBook.Application.UseCases.Recipe.Update;

public class UpdateRecipeValidator : AbstractValidator<RequestRecipeJson>
{
    public UpdateRecipeValidator()
    {
        RuleFor(x => x).SetValidator(new RecipeValidator());
    }
}
