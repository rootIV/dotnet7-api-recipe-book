using HashidsNet;
using MyRecipeBook.Application.Services.UserLogged;
using MyRecipeBook.Domain.Repositorys;
using MyRecipeBook.Domain.Repositorys.Code;

namespace MyRecipeBook.Application.UseCases.Connection.RefuseConnection;

public class RefuseConnectionUseCase : IRefuseConnectionUseCase
{
    private readonly ICodeWriteOnlyRepository _codeWriteRepository;
    private readonly IUserLogged _userLogged;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IHashids _hashids;

    public RefuseConnectionUseCase(ICodeWriteOnlyRepository codeReadRepository,
        IUserLogged userLogged,
        IUnityOfWork unityOfWork,
        IHashids hashids)
    {
        _codeWriteRepository = codeReadRepository;
        _userLogged = userLogged;
        _unityOfWork = unityOfWork;
        _hashids = hashids;
    }

    public async Task<string> Execute()
    {
        var userLogged = await _userLogged.RecoverUserLoggedToken();

        await _codeWriteRepository.Delete(userLogged.Id);

        await _unityOfWork.Commit();

        return _hashids.EncodeLong(userLogged.Id);
    }
}
