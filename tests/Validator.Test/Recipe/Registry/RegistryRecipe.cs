using FluentAssertions;
using MyRecipeBook.Application.UseCases.Recipe.Registry;
using MyRecipeBook.Communication.Enum;
using MyRecipeBook.Exceptions;
using Unity.Test.Utils.Requests;
using Xunit;

namespace Validator.Test.Recipe.Registry;

public class RegistryRecipe
{
    [Fact]
    public void Validate_Succcess()
    {
        var validator = new RegistryRecipeValidator();

        var request = RequestRegistryRecipeBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }
    [Fact]
    public void Validate_Recipe_Title_Empty_Erro()
    {
        var validator = new RegistryRecipeValidator();

        var request = RequestRegistryRecipeBuilder.Build();
        request.Title = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(erro => erro.ErrorMessage.Equals(ErroMessagesResource.User_Recipe_Title_Empty));
    }
    [Fact]
    public void Validate_Recipe_Category_Empty_Erro()
    {
        var validator = new RegistryRecipeValidator();

        var request = RequestRegistryRecipeBuilder.Build();
        request.Category = (Category)9999;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(erro => erro.ErrorMessage.Equals(ErroMessagesResource.User_Recipe_Category_Invalid));
    }
    [Fact]
    public void Validate_Recipe_Preparation_Method_Empty_Erro()
    {
        var validator = new RegistryRecipeValidator();

        var request = RequestRegistryRecipeBuilder.Build();
        request.PreparationMethod = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(erro => erro.ErrorMessage.Equals(ErroMessagesResource.User_Recipe_Preparation_Method_Empty));
    }
    [Fact]
    public void Validate_Recipe_Ingredients_Empty_Erro()
    {
        var validator = new RegistryRecipeValidator();

        var request = RequestRegistryRecipeBuilder.Build();
        request.Ingredients.Clear();

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(erro => erro.ErrorMessage.Equals(ErroMessagesResource.User_Recipe_Ingredients_Empty));
    }
    [Fact]
    public void Validate_Recipe_Ingredients_Product_Empty_Erro()
    {
        var validator = new RegistryRecipeValidator();

        var request = RequestRegistryRecipeBuilder.Build();
        request.Ingredients.First().Product = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(erro => erro.ErrorMessage.Equals(ErroMessagesResource.User_Recipe_Ingredients_Product_Empty));
    }
    [Fact]
    public void Validate_Recipe_Ingredients_Quantity_Empty_Erro()
    {
        var validator = new RegistryRecipeValidator();

        var request = RequestRegistryRecipeBuilder.Build();
        request.Ingredients.First().Quantity = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(erro => erro.ErrorMessage.Equals(ErroMessagesResource.User_Recipe_Ingredients_Quantity_Empty));
    }
    [Fact]
    public void Validate_Recipe_Ingredients_Duplied_Erro()
    {
        var validator = new RegistryRecipeValidator();

        var request = RequestRegistryRecipeBuilder.Build();
        request.Ingredients.Add(request.Ingredients.First());

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(erro => erro.ErrorMessage.Equals(ErroMessagesResource.User_Recipe_Ingredients_Product_Duplicate_Erro));
    }
}
