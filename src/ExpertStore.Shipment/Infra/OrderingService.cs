using ExpertStore.Shipment.Application.Integration;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace ExpertStore.Shipment.Infra;

public class OrderingService : IOrderingService
{
    private readonly string OrderingServiceUrl;
    private readonly ILogger<OrderingService> _logger; 

    public OrderingService(IConfiguration config, ILogger<OrderingService> logger) 
        => (OrderingServiceUrl, _logger) = (config.GetValue<string>("OrdersServiceUrl"), logger);

    public async Task<OrderDto?> GetOrderDetails(Guid orderId)
    {
        try
        {
            _logger.LogInformation($"Calling ordering service to get the order: {orderId}");
            var orderDtoJson = await OrderingServiceUrl
                .AppendPathSegment($"/api/orders/{orderId}")
                .GetStringAsync();
            var orderDto = JsonConvert.DeserializeObject<OrderDto>(orderDtoJson);

            _logger.LogInformation($"Get response ok with orderId {orderDto.Id}");
            return orderDto;
        }
        catch (FlurlHttpException ex)
        {
            if (ex.Call.Response.StatusCode == StatusCodes.Status404NotFound)
                return null;

            var error = await ex.GetResponseStringAsync();
            _logger.LogError($"Error returned from {ex.Call.Request.Url}: {error}");
            throw new Exception(error);
        }
    }
}

