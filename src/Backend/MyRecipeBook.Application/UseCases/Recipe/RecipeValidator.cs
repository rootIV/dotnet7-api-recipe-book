using FluentValidation;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Domain.Extension;
using MyRecipeBook.Exceptions;

namespace MyRecipeBook.Application.UseCases.Recipe;

public class RecipeValidator : AbstractValidator<RequestRecipeJson>
{
    public RecipeValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage(ErroMessagesResource.User_Recipe_Title_Empty);
        RuleFor(x => x.Category).IsInEnum().WithMessage(ErroMessagesResource.User_Recipe_Category_Invalid);
        RuleFor(x => x.PreparationMethod).NotEmpty().WithMessage(ErroMessagesResource.User_Recipe_Preparation_Method_Empty);
        RuleFor(x => x.Ingredients).NotEmpty().WithMessage(ErroMessagesResource.User_Recipe_Ingredients_Empty);
        RuleFor(x => x.PreparationTime).InclusiveBetween(1, 1000).WithMessage(ErroMessagesResource.User_Recipe_Preparation_Time_Invalid);

        RuleForEach(x => x.Ingredients).ChildRules(ingredient =>
        {
            ingredient.RuleFor(x => x.Product).NotEmpty().WithMessage(ErroMessagesResource.User_Recipe_Ingredients_Product_Empty);
            ingredient.RuleFor(x => x.Quantity).NotEmpty().WithMessage(ErroMessagesResource.User_Recipe_Ingredients_Quantity_Empty);
        });

        RuleFor(x => x.Ingredients).Custom((ingredients, context) =>
        {
            var distinctProductsCount = ingredients.Select(c => c.Product.RemoveAccents().ToLower()).Distinct();
            if (distinctProductsCount.Count() != ingredients.Count)
                context.AddFailure(new FluentValidation.Results.ValidationFailure("Ingredients", ErroMessagesResource.User_Recipe_Ingredients_Product_Duplicate_Erro));
        });
    }
}
