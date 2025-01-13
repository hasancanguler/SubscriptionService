using Infrastructure.Contract;
using Infrastructure.Repository.Entities;
using Infrastructure.Service;
using Infrastructure.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace SubscriptionAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ILogger<SubscriptionController> _logger;
        private readonly ISubscription _subscription;

        public SubscriptionController(ILogger<SubscriptionController> logger,
            ISubscription subscription)
        {
            _logger = logger;
            _subscription = subscription;
        }

        [HttpPost("Subscribe")]
        public async Task<SubscribeResponse> Subscribe(SubscribeRequest subscribeRequest)
        {
            return await _subscription.Subscribe(subscribeRequest);
        }

        [HttpPost("Unsubscribe")]
        public async Task<SubscribeResponse> Unsubscribe(SubscribeRequest subscribeRequest)
        {
            return await _subscription.Unsubscribe(subscribeRequest);
        }

        [HttpPost("SubscriptionSummary")]
        public async Task<List<SubscriptionModel>> SubscriptionSummary(SubscriptionSummaryRequest subscriptionSummaryRequest)
        {
            return await _subscription.SubscriptionSummary(subscriptionSummaryRequest);
        }
    }
}
