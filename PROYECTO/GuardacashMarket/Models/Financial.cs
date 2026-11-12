using System;

namespace GuardacashMarket.Models;

public class Gain
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public DateTime Date { get; set; } = DateTime.Now;
	public string Description { get; set; } = "";
	public decimal Amount { get; set; }
	public decimal Cost { get; set; }
	public decimal Profit => Amount - Cost;
}

public class Quote
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public DateTime Date { get; set; } = DateTime.Now;
	public string ClientName { get; set; } = "";
	public string ServiceName { get; set; } = "";
	public decimal MaterialCost { get; set; }
	public decimal LaborCost { get; set; }
	public decimal Total => MaterialCost + LaborCost;
	public decimal SuggestedPrice => Math.Round(Total * 1.25m, 0);
}

public class Order
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public DateTime Date { get; set; } = DateTime.Now;
	public string ClientName { get; set; } = "";
	public string ServiceName { get; set; } = "";
	public string Status { get; set; } = "Pendiente";
	public decimal Amount { get; set; }
}

public class Investment
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public DateTime Date { get; set; } = DateTime.Now;
	public string Label { get; set; } = "";
	public decimal Amount { get; set; }
	public decimal ProjectedReturn { get; set; } = 0.18m;
	public decimal ProjectedProfit => Math.Round(Amount * ProjectedReturn, 2);
}