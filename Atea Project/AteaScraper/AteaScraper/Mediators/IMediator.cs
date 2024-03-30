using System.Threading.Tasks;

namespace AteaScraper.Mediators
{
    public interface IMediator
    {
        Task<object> Send<TRequest>(TRequest request);
    }
}
