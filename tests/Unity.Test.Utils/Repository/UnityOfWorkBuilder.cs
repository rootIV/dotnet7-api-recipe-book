using Moq;
using MyRecipeBook.Domain.Repositorys;

namespace Unity.Test.Utils.Repository;

public class UnityOfWorkBuilder
{
    private static UnityOfWorkBuilder _instance;
    private readonly Mock<IUnityOfWork> _repository;

    private UnityOfWorkBuilder()
    {
        _repository ??= new Mock<IUnityOfWork>();
    }

    public static UnityOfWorkBuilder Instance()
    {
        _instance = new UnityOfWorkBuilder();
        return _instance;
    }
    public IUnityOfWork Build()
    {
        return _repository.Object;
    }
}
