using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Registry;
using MyRecipeBook.Exceptions;
using Unity.Test.Utils.Requests;
using Xunit;

namespace Validator.Test.User.Registry;

public class RegistryUser
{
    [Fact]
    public void Validate_Succcess()
    {
        var validator = new RegistryUserValidator();

        var requisition = RequestRegistryUserBuilder.Build();

        var result = validator.Validate(requisition);

        result.IsValid.Should().BeTrue();
    }
    [Fact]
    public void Validate_Erro_Empty_Name()
    {
        var validator = new RegistryUserValidator();

        var requisition = RequestRegistryUserBuilder.Build();
        requisition.Name = string.Empty;

        var result = validator.Validate(requisition);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(erro => erro.ErrorMessage.Equals(ErroMessagesResource.User_Name_Empty));
    }
    [Fact]
    public void Validate_Erro_Empty_Email()
    {
        var validator = new RegistryUserValidator();

        var requisition = RequestRegistryUserBuilder.Build();
        requisition.Email = string.Empty;

        var result = validator.Validate(requisition);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(erro => erro.ErrorMessage.Equals(ErroMessagesResource.User_Email_Empty));
    }
    [Fact]
    public void Validate_Erro_Empty_Phone()
    {
        var validator = new RegistryUserValidator();

        var requisition = RequestRegistryUserBuilder.Build();
        requisition.Phone = string.Empty;

        var result = validator.Validate(requisition);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(erro => erro.ErrorMessage.Equals(ErroMessagesResource.User_Phone_Empty));
    }
    [Fact]
    public void Validate_Erro_Empty_Password()
    {
        var validator = new RegistryUserValidator();

        var request = RequestRegistryUserBuilder.Build();
        request.Password = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(erro => erro.ErrorMessage.Equals(ErroMessagesResource.User_Password_Empty));
    }
    [Fact]
    public void Validate_Erro_Invalid_Email()
    {
        var validator = new RegistryUserValidator();

        var requisition = RequestRegistryUserBuilder.Build();
        requisition.Email = "vaness";

        var result = validator.Validate(requisition);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(erro => erro.ErrorMessage.Equals(ErroMessagesResource.User_Email_Invalid));
    }
    [Fact]
    public void Validate_Erro_Invalid_Phone()
    {
        var validator = new RegistryUserValidator();

        var requisition = RequestRegistryUserBuilder.Build();
        requisition.Phone = "33 9";

        var result = validator.Validate(requisition);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(erro => erro.ErrorMessage.Equals(ErroMessagesResource.User_Phone_Invalid));
    }
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Validate_Erro_Invalid_Password(int passwordLength)
    {
        var validator = new RegistryUserValidator();

        var requisition = RequestRegistryUserBuilder.Build(passwordLength);

        var result = validator.Validate(requisition);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(erro => erro.ErrorMessage.Equals(ErroMessagesResource.User_Password_MinChar));
    }
}
