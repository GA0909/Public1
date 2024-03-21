using System;
using System.Threading.Tasks;

namespace AteaScraper.Mediators
{
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public Mediator()
        {
        }

        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<object> Send<TRequest>(TRequest request)
        {
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(typeof(TRequest), typeof(object));
            dynamic handler = _serviceProvider.GetService(handlerType);
            return await handler.Handle((dynamic)request);
        }
    }
}
