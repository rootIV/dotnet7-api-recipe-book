using HashidsNet;
using MyRecipeBook.Application.Services.UserLogged;
using MyRecipeBook.Domain.Repositorys;
using MyRecipeBook.Domain.Repositorys.Code;
using QRCoder;
using System.Drawing;

namespace MyRecipeBook.Application.UseCases.Connection.GenerateQRCode;

public class GenerateQRCodeUseCase : IGenerateQRCodeUseCase
{
    private readonly ICodeWriteOnlyRepository _codeWriteRepository;
    private readonly IUserLogged _userLogged;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IHashids _hashIds;

    public GenerateQRCodeUseCase(ICodeWriteOnlyRepository codeWriteRepository, 
        IUserLogged userLogged, 
        IUnityOfWork unityOfWork,
        IHashids hashIds)
    {
        _codeWriteRepository = codeWriteRepository;
        _userLogged = userLogged;
        _unityOfWork = unityOfWork;
        _hashIds = hashIds;
    }

    public async Task<(byte[] qrCode, string userId)> Execute()
    {
        var userLogged = await _userLogged.RecoverUserLoggedToken();

        var code = new Domain.Entities.Codes
        {
            Code = Guid.NewGuid().ToString(),
            UserId = userLogged.Id
        };

        await _codeWriteRepository.Registry(code);
        await _unityOfWork.Commit();

        return (GenerateQRCodeImage(code.Code), _hashIds.EncodeLong(userLogged.Id));
    }

    private static byte[] GenerateQRCodeImage(string code) 
    {
        var qrCodeGenerator = new QRCodeGenerator();

        var qrCodeData = qrCodeGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);

        var qrCode = new QRCode(qrCodeData);

        var bitmap = qrCode.GetGraphic(5, Color.Black, Color.Transparent, true);

        using var stream = new MemoryStream();

        bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

        return stream.ToArray();
    }
}
