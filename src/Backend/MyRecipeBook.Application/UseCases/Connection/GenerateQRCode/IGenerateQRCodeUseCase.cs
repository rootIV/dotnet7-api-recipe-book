using System.Drawing;

namespace MyRecipeBook.Application.UseCases.Connection.GenerateQRCode;

public interface IGenerateQRCodeUseCase
{
    Task<(byte[] qrCode, string userId)> Execute();
}
