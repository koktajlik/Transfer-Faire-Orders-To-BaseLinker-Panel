using System.Collections.Generic;
using Newtonsoft.Json;

namespace TransferFromFaireToBaseLinker;

public class ExistingBaseLinkerOrders
{
    [JsonProperty("orders")]
    public List<BaseLinkerOrder> Orders { get; set; }

    ExistingBaseLinkerOrders()
    {
        Orders = new List<BaseLinkerOrder>();
    }
}

public class BaseLinkerOrder
{
    [JsonProperty("order_status_id")]
    public int OrderStatusId { get; set; }
    
    [JsonProperty("custom_source_id")]
    public int CustomSourceId { get; set; }
    
    [JsonProperty("date_add")]
    public int DateAdd { get; set; }

    [JsonProperty("phone")]
    public string Phone { get; set; }

    [JsonProperty("currency")]
    public string Currency { get; set; }

    [JsonProperty("delivery_fullname")]
    public string DeliveryFullname { get; set; }
    
    [JsonProperty("delivery_company")]
    public string DeliveryCompany { get; set; }
    
    [JsonProperty("delivery_address")]
    public string DeliveryAddress { get; set; }
    
    [JsonProperty("delivery_city")]
    public string DeliveryCity { get; set; }
    
    [JsonProperty("delivery_state")]
    public string DeliveryState { get; set; }
    
    [JsonProperty("delivery_postcode")]
    public string DeliveryPostcode { get; set; }
    
    [JsonProperty("delivery_country_code")]
    public string DeliveryCountryCode { get; set; }

    [JsonProperty("want_invoice")]
    public bool WantInvoice { get; set; }
    
    [JsonProperty("extra_field_1")]
    public string ExtraField1 { get; set; }

    [JsonProperty("products")]
    public List<Product> BaseLinkerProducts { get; set; }

    public BaseLinkerOrder()
    {
        BaseLinkerProducts = new List<Product>();
    }
}
public class Product
{
    [JsonProperty("product_id")]
    public string ProductId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("sku")]
    public string Sku { get; set; }

    [JsonProperty("price_brutto")]
    public decimal PriceBrutto { get; set; }
    
    [JsonProperty("quantity")]
    public int Quantity { get; set; }
}
