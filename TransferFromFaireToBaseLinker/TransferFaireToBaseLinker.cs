using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace TransferFromFaireToBaseLinker;

public static class TransferFaireToBaseLinker
{
    [FunctionName("TransferFaireToBaseLinker")]
    public static async Task RunAsync([TimerTrigger("0 */10 * * * *", RunOnStartup = true)] TimerInfo myTimer, ILogger log)
    {
         try
         {
             var transferOrders = new TransferOperations();
             var faireOrders = await transferOrders.FetchFaireOrders();
             if (faireOrders != null)
             {
                 const int baseLinkerStatus = 8069;
                 foreach (var order in faireOrders.FaireListOrders)
                 {
                     if (await transferOrders.CheckIfOrderExists(order.Id, baseLinkerStatus)) continue;
                     
                     // Create BaseLinker order from Faire order
                     var baseLinkerOrder = new BaseLinkerOrder
                     {
                         OrderStatusId = baseLinkerStatus,
                         CustomSourceId = 1024,
                         DateAdd = AdditionalOperations.ConvertFaireDateToUnix(order.CreatedAt),
                         Phone = order.Address.PhoneNumber,
                         Currency = "USD",
                         DeliveryFullname = order.Address.Name,
                         DeliveryCompany = order.Address.CompanyName,
                         DeliveryAddress = $"{order.Address.Address1};{order.Address.Address2}",
                         DeliveryCity = order.Address.City,
                         DeliveryState = order.Address.State,
                         DeliveryPostcode = order.Address.PostalCode,
                         DeliveryCountryCode = AdditionalOperations.Alpha3ToAlpha2(order.Address.CountryCode),
                         WantInvoice = false,
                         ExtraField1 = order.Id
                     };

                     // Add one or more products to BaseLinker order
                     foreach (var item in order.Items)
                     {
                         var product = new Product
                         {
                             ProductId = item.ProductId,
                             Name = item.ProductName,
                             Sku = item.Sku,
                             PriceBrutto = Convert.ToDecimal(item.PriceCents / 100),
                             Quantity = item.Quantity
                         };
                         baseLinkerOrder.BaseLinkerProducts.Add(product);
                     }
                     
                     await transferOrders.PostBaseLinkerOrder(baseLinkerOrder);
                 }

                 log.LogInformation("All orders have been successfully added to BaseLink panel");
                 return;
             }

             log.LogInformation("Orders list have been empty.");
         }
         catch (Exception ex)
         {
             throw new Exception("Problems have occurred during transfer orders.", ex);
         }
        
    }
}