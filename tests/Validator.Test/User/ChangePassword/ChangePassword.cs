using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Exceptions;
using Unity.Test.Utils.Requests;
using Xunit;

namespace Validator.Test.User.ChangePassword;

public class ChangePassword
{
    [Fact]
    public void Validate_Succcess()
    {
        var validator = new ChangePasswordValidator();

        var requisition = RequestChangePasswordBuilder.Build();

        var result = validator.Validate(requisition);

        result.IsValid.Should().BeTrue();
    }
    [Fact]
    public void Validate_Erro_Empty_Password()
    {
        var validator = new ChangePasswordValidator();

        var request = RequestChangePasswordBuilder.Build();
        request.NewPassword = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(erro => erro.ErrorMessage.Equals(ErroMessagesResource.User_Password_Empty));
    }
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Validate_Erro_Invalid_Password(int passwordLength)
    {
        var validator = new ChangePasswordValidator();

        var requisition = RequestChangePasswordBuilder.Build(passwordLength);

        var result = validator.Validate(requisition);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(erro => erro.ErrorMessage.Equals(ErroMessagesResource.User_Password_MinChar));
    }
}
