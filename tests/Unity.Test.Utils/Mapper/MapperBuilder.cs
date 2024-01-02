using AutoMapper;
using MyRecipeBook.Application.Services.AutoMapper;
using Unity.Test.Utils.HashIds;

namespace Unity.Test.Utils.Mapper;

public class MapperBuilder
{
    public static IMapper Instance()
    {
        var hashIds = HashIdsBuilder.Instance().Build();

        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutoMapperConfigurator(hashIds));
        });

        return configuration.CreateMapper();
    }
}
