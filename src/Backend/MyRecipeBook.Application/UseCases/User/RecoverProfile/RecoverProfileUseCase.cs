using AutoMapper;
using MyRecipeBook.Application.Services.UserLogged;
using MyRecipeBook.Communication.Response;

namespace MyRecipeBook.Application.UseCases.User.RecoverProfile;

public class RecoverProfileUseCase : IRecoverProfileUseCase
{
    private readonly IMapper _mapper;
    private readonly IUserLogged _userLogged;

    public RecoverProfileUseCase(IMapper mapper, IUserLogged userLogged)
    {
        _mapper = mapper;
        _userLogged = userLogged;
    }

    public async Task<ResponseUserProfileJson> Execute()
    {
        var user = await _userLogged.RecoverUserLoggedToken();

        return _mapper.Map<ResponseUserProfileJson>(user);
    }
}
