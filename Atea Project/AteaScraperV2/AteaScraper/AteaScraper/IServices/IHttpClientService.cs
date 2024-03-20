using System.Net.Http;
using System.Threading.Tasks;

namespace AteaScraper.IServices
{
    public interface IHttpClientService
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }
}
