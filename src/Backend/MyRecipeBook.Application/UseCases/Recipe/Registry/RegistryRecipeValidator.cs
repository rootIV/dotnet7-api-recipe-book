using FluentValidation;
using MyRecipeBook.Communication.Request;

namespace MyRecipeBook.Application.UseCases.Recipe.Registry;

public class RegistryRecipeValidator : AbstractValidator<RequestRecipeJson>
{
    public RegistryRecipeValidator()
    {
        RuleFor(x => x).SetValidator(new RecipeValidator());
    }
}
