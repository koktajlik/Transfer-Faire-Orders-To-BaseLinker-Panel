using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace TransferFromFaireToBaseLinker;

public class TransferOperations
{
    // BaseLinker API
    private readonly RestClient _baseLinkerClient;
    // Faire API
    private readonly RestClient _faireClient;

    public TransferOperations()
    {
        _baseLinkerClient = new RestClient(Environment.GetEnvironmentVariable("BaseLinkerAPIUrl") ??
                                           throw new Exception("BaseLinker API url is not valid."));
        _faireClient = new RestClient(Environment.GetEnvironmentVariable("FaireAPIUrl") ??
                                      throw new Exception("Faire API url is not valid."));
    }

    /// <summary>
    /// Create Faire object from fetched Faire orders
    /// </summary>
    /// <returns>Faire Order object</returns>
    public async Task<FaireOrder?> FetchFaireOrders()
    {
        var request = new RestRequest("orders")
        {
            Method = Method.Get
        };
        var faireToken = Environment.GetEnvironmentVariable("X-FAIRE-ACCESS-TOKEN") ??
                         throw new Exception("Faire API token not provided.");
        request.AddHeader("X-FAIRE-ACCESS-TOKEN", faireToken);

        var response = await _faireClient.ExecuteAsync(request);
        if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content))
        {
            throw new Exception("Faire response failed or response was null.", response.ErrorException);
        }

        var faireOrder = JsonConvert.DeserializeObject<FaireOrder>(response.Content);
        return faireOrder;

    }

    /// <summary>
    /// Post BaseLinker order object to BaseLinker panel
    /// </summary>
    /// <param name="order">BaseLinker order object</param>
    public async Task PostBaseLinkerOrder(BaseLinkerOrder order)
    {
        var orderToTransfer = JsonConvert.SerializeObject(order);
        var request = new RestRequest
        {
            Method = Method.Post
        };
        var baseLinkerToken = Environment.GetEnvironmentVariable("X-BLToken") ??
                              throw new Exception("BaseLinker API token not provided.");
        request.AddHeader("X-BLToken", baseLinkerToken);
        request.AddParameter("method", "addOrder");
        request.AddParameter("parameters", orderToTransfer);

        var response = await _baseLinkerClient.ExecuteAsync(request);
        if (!response.IsSuccessful || string.IsNullOrEmpty(response.Content))
        {
            throw new Exception("Error while adding order.", response.ErrorException);
        }
    }

    /// <summary>
    /// Check if the added Faire order exists in BaseLinker panel
    /// </summary>
    /// <param name="faireOrderId">Order id from Faire marketplace</param>
    /// <param name="statusId">BaseLinker status id</param>
    /// <returns>True if order exists, otherwise false</returns>
    public async Task<bool> CheckIfOrderExists(string faireOrderId, int statusId)
    {
        var request = new RestRequest
        {
            Method = Method.Get
        };
        request.AddHeader("X-BLToken", Environment.GetEnvironmentVariable("X-BLToken") ?? string.Empty);
        request.AddParameter("method", "getOrders");
        request.AddParameter("status_id", statusId);
        request.AddParameter("include_custom_extra_fields", true);
        
        var response = await _baseLinkerClient.ExecuteAsync(request);
        if (response.IsSuccessful || !string.IsNullOrEmpty(response.Content))
        {
            var baseLinkerOrders = JsonConvert.DeserializeObject<ExistingBaseLinkerOrders>(response.Content);
            return baseLinkerOrders!.Orders.Any(x => x.ExtraField1 == faireOrderId);
        }
        return false;
    }
}