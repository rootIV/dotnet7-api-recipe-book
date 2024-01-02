namespace Unity.Test.Utils.HashIds;

public class HashIdsBuilder
{
    private static HashIdsBuilder _instance;
    private readonly HashidsNet.Hashids _enc;

    private HashIdsBuilder()
    {
        _enc ??= new HashidsNet.Hashids("vt30F0dAkK", 3);
    }

    public static HashIdsBuilder Instance()
    {
        _instance = new HashIdsBuilder();
        return _instance;
    }
    public HashidsNet.Hashids Build()
    {
        return _enc;
    }
}
