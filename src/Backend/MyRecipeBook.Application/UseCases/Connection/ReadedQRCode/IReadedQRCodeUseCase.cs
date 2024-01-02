using MyRecipeBook.Communication.Response;

namespace MyRecipeBook.Application.UseCases.Connection.ReadedQRCode;

public interface IReadedQRCodeUseCase
{
    Task<(ResponseUserConnectionJson ConnectingUser, string QRCodeGeneratorUserId)> Execute(string connectionCode);
}
