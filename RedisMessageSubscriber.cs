
using StackExchange.Redis;

namespace BankTrackingSystem
{
    public class RedisMessageSubscriber : BackgroundService
    {
        private readonly ILogger<RedisMessageSubscriber> _logger;
        private readonly IConfiguration _configuration;
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        private static readonly RedisChannel _channel = RedisChannel.Pattern("bank-account-status-changed-channel");

        public RedisMessageSubscriber(IConfiguration configuration, ILogger<RedisMessageSubscriber> logger)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionMultiplexer = ConnectionMultiplexer.Connect("localhost:6379,password=redispw,user=default");
            //_connectionMultiplexer = ConnectionMultiplexer.Connect(_configuration.GetConnectionString("SQLConnectionString"));
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var subscriber = _connectionMultiplexer.GetSubscriber();
            await subscriber.SubscribeAsync(_channel, (channel, message) =>
            {
                _logger.LogInformation("Received message: {Channel} {Message}", channel, message);
            });
        }
    }
}
