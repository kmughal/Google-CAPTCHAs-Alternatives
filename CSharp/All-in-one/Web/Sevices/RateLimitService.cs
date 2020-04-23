namespace Web.Services
{
    using System;
    using System.Net;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;

    public interface IRateLimitService
    {
        bool IsRequestValidToProceed(IPAddress requestIpAddress);
    }

    public class RateLimitService : IRateLimitService
    {
        private struct ClientRequestInformation
        {
            public IPAddress ClientAddress { get; set; }
            public DateTime RequestDateTime { get; set; }
        }

        private int EXPIRE_TIME_IN_SECONDS = 10;

        private readonly IMemoryCache _cache;
        private readonly ILogger<RateLimitService> _logger;
        private const int EXPIRATION_TIME_IN_SECONDS = 10;

        public RateLimitService(IMemoryCache cache, ILogger<RateLimitService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public bool IsRequestValidToProceed(IPAddress requestIpAddress)
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
                return true;
            }
            else
            {
                _logger.LogInformation($"{requestIpAddress.ToString()}, Request time: {clientRequestObject.RequestDateTime} , Diff: {DateTime.UtcNow.Subtract(clientRequestObject.RequestDateTime).Seconds}");
                _logger.LogInformation($"Request is not valid");
                return false;
            }


            MemoryCacheEntryOptions GetCacheExpirationPolicy() => new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(DateTimeOffset.UtcNow.AddSeconds(EXPIRE_TIME_IN_SECONDS));
        }

    }
}