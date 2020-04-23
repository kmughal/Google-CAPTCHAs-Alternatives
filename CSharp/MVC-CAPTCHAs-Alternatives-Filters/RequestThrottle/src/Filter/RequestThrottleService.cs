namespace RequestThrottle.Filter
{
    using System;
    using System.Net;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;

    public interface IRequestThrottleService
    {
        void IsRequestValidToProceed(IPAddress requestIpAddress);
        void SetExpirationTime(int expirationTime);
    }


    public class RequestThrottleService : IRequestThrottleService
    {
        private struct ClientRequestInformation
        {
            public IPAddress ClientAddress { get; set; }
            public DateTime RequestDateTime { get; set; }
        }

        private int EXPIRE_TIME_IN_SECONDS = 30;

        private readonly IMemoryCache _cache;
        private readonly ILogger<RequestThrottleService> _logger;

        public RequestThrottleService(IMemoryCache cache, ILogger<RequestThrottleService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public void SetExpirationTime(int expirationTime)
        {
            EXPIRE_TIME_IN_SECONDS = expirationTime;
        }

        public void IsRequestValidToProceed(IPAddress requestIpAddress)
        {
            var lookupResult = _cache.TryGetValue(requestIpAddress.ToString(), out ClientRequestInformation clientRequestObject);
            if (!lookupResult)
            {
                _logger.LogInformation($"{requestIpAddress.ToString()} not present in the cache");

                var cacheEntry = new ClientRequestInformation
                {
                    ClientAddress = requestIpAddress,
                    RequestDateTime = DateTime.UtcNow
                };
                _cache.Set(requestIpAddress.ToString(), cacheEntry, GetCacheExpirationPolicy());
            }
            else
            {
                _logger.LogInformation($"{requestIpAddress.ToString()}, Request time: {clientRequestObject.RequestDateTime} , Diff: {DateTime.UtcNow.Subtract(clientRequestObject.RequestDateTime).Seconds}");
                throw new InvalidOperationException("Too many requests");
            }


            MemoryCacheEntryOptions GetCacheExpirationPolicy() => new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(DateTimeOffset.UtcNow.AddSeconds(EXPIRE_TIME_IN_SECONDS));
        }

    }
}