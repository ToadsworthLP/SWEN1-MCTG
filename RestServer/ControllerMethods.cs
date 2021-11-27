using Rest.Attributes;

namespace Rest
{
    public interface IControllerMethod { }

    public interface IGet<T> : IControllerMethod
    {
        [Method(Method.GET)]
        IApiResponse Get(T request);
    }

    public interface IPost<T> : IControllerMethod
    {
        [Method(Method.POST)]
        IApiResponse Post(T request);
    }

    public interface IPut<T> : IControllerMethod
    {
        [Method(Method.PUT)]
        IApiResponse Put(T request);
    }
}
