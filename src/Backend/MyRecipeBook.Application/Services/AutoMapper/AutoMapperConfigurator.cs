using AutoMapper;
using HashidsNet;

namespace MyRecipeBook.Application.Services.AutoMapper;

public class AutoMapperConfigurator : Profile
{
    private readonly IHashids _hashIds;

    public AutoMapperConfigurator(IHashids hashIds)
    {
        _hashIds = hashIds;

        EntitieRequest();
        EntitieResponse();
    }

    private void EntitieRequest()
    {
        CreateMap<Communication.Request.RequestRegistryUserJson, Domain.Entities.User>()
            .ForMember(destination => destination.Password, config => config.Ignore());

        CreateMap<Communication.Request.RequestRecipeJson, Domain.Entities.Recipe>();
        CreateMap<Communication.Request.RequestIngredientJson, Domain.Entities.Ingredient>();
    }
    private void EntitieResponse()
    {
        CreateMap<Domain.Entities.Recipe, Communication.Response.ResponseRecipeJson>()
            .ForMember(destination => destination.Id, config => config.MapFrom(origin => _hashIds.EncodeLong(origin.Id)));

        CreateMap<Domain.Entities.Ingredient, Communication.Response.ResponseIngredientJson>()
            .ForMember(destination => destination.Id, config => config.MapFrom(origin => _hashIds.EncodeLong(origin.Id)));

        CreateMap<Domain.Entities.Recipe, Communication.Response.ResponseDashboardRecipeJson>()
            .ForMember(destination => destination.Id, config => config.MapFrom(origin => _hashIds.EncodeLong(origin.Id)))
            .ForMember(destination => destination.IngredientsQuantity, config => config.MapFrom(origin => origin.Ingredients.Count));

        CreateMap<Domain.Entities.User, Communication.Response.ResponseUserProfileJson>();

        CreateMap<Domain.Entities.User, Communication.Response.ResponseUserConnectedJson>()
            .ForMember(destination => destination.Id, config => config.MapFrom(origin => _hashIds.EncodeLong(origin.Id)));
    }
}
