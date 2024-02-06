
using BankTrackingSystem.Data;
using BankTrackingSystem.Models;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Security.Cryptography;

namespace BankTrackingSystem
{
    public class RedisMessageSubscriber : BackgroundService
    {
        private readonly ILogger<RedisMessageSubscriber> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        private static readonly RedisChannel _channel = RedisChannel.Pattern("bank-account-status-changed-channel");

        public RedisMessageSubscriber(IConfiguration configuration, ILogger<RedisMessageSubscriber> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _connectionMultiplexer = ConnectionMultiplexer.Connect("localhost:6379,password=redispw,user=default");
            //_connectionMultiplexer = ConnectionMultiplexer.Connect(_configuration.GetConnectionString("SQLConnectionString"));
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var subscriber = _connectionMultiplexer.GetSubscriber();

            var scope = _serviceProvider.CreateAsyncScope();

            var applicantRepository = scope.ServiceProvider.GetRequiredService<IApplicantMessagesRespository>();

            try
            {
                // Subscribe to the channel
                await subscriber.SubscribeAsync(_channel, async (channel, message) =>
                {
                    var applicantMessage = JsonConvert.DeserializeObject<ApplicantMessagesModel>(message);
                    await applicantRepository.AddMessage(applicantMessage);
                    _logger.LogInformation("Received message: {Channel} {Message}", channel, message);

                    // Remove
                    //var testMessage = new ApplicantMessagesModel();
                    //testMessage.Id = Guid.NewGuid();
                    //testMessage.ApplicantId = GenerateRandomLong();
                    //testMessage.Message = Guid.NewGuid().ToString();
                    //await applicantRepository.AddMessage(testMessage);
                });

                // Wait until cancellation is requested
                await Task.Delay(-1, stoppingToken);
            }
            finally
            {
                // Dispose of the scope when done
                scope.Dispose();
            }
        }


        static long GenerateRandomLong()
        {
            byte[] bytes = new byte[8];
            new Random().NextBytes(bytes);
            long randomLong = BitConverter.ToInt64(bytes, 0);
            return randomLong;
        }
    }
}
