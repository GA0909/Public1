using System.Threading.Tasks;

namespace AteaScraper.Mediators
{
    public interface IRequestHandler<TRequest, TResult>
    {
        Task<TResult> Handle(TRequest request);
    }
}
