namespace GuardacashMarket.Models;

public class Service
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string Name { get; set; } = "";
	public string OwnerId { get; set; } = "";
	public string OwnerName { get; set; } = "";
	public string Category { get; set; } = "";
	public string Description { get; set; } = "";
	public decimal Price { get; set; }
	public decimal Change { get; set; }
	public double Rating { get; set; }
	public int Reviews { get; set; }
	public bool IsVerified { get; set; }
	public string Icon => Category switch
	{
		"Foto" => "📷",
		"Diseño" => "🎨",
		"Edición" => "🎬",
		"Manualidades" => "✂️",
		"Tecnología" => "💻",
		"Música" => "🎵",
		"Reparaciones" => "🔧",
		"Maquillaje" => "💄",
		_ => "✨"
	};

	public bool IsPositive => Change >= 0;
	public string ChangeDisplay => Change >= 0 ? $"▲ +{Change:N1}%" : $"▼ {Change:N1}%";
}