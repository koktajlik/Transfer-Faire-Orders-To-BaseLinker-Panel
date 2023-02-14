#nullable enable
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TransferFromFaireToBaseLinker;

public class FaireOrder
{
    [JsonProperty("orders")]
    public List<Order>? FaireListOrders { get; set; }
}

public class Item 
{
    [JsonProperty("product_id")]
    public string ProductId { get; set; }

    [JsonProperty("quantity")]
    public int Quantity { get; set; }

    [JsonProperty("sku")]
    public string Sku { get; set; }

    [JsonProperty("price_cents")]
    public int PriceCents { get; set; }

    [JsonProperty("product_name")]
    public string ProductName { get; set; }
}

public class Address
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("address1")]
    public string Address1 { get; set; }

    [JsonProperty("address2")]
    public string Address2 { get; set; }

    [JsonProperty("postal_code")]
    public string PostalCode { get; set; }

    [JsonProperty("city")]
    public string City { get; set; }

    [JsonProperty("state")]
    public string State { get; set; }

    [JsonProperty("phone_number")]
     public string PhoneNumber { get; set; }

     [JsonProperty("country_code")]
    public string CountryCode { get; set; }

    [JsonProperty("company_name")]
    public string CompanyName { get; set; }
}

public class Order
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("created_at")]
    public string CreatedAt { get; set; }

    [JsonProperty("items")]
    public List<Item> Items { get; set; }

    [JsonProperty("address")]
    public Address Address { get; set; }
    
}
