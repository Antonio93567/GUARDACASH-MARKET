using System.Text.Json;
using GuardacashMarket.Models;

namespace GuardacashMarket.Services;

public interface IDataService
{
	Task InitializeAsync();
	string DataRoot { get; }

	Task<User?> GetProfileAsync();
	Task SaveProfileAsync(User user);

	Task<List<Service>> GetServicesAsync();
	Task<List<Gain>> GetGainsAsync();
	Task AddGainAsync(Gain gain);
	Task<List<Quote>> GetQuotesAsync();
	Task AddQuoteAsync(Quote quote);
	Task<List<Order>> GetOrdersAsync();
	Task AddOrderAsync(Order order);
	Task<List<Investment>> GetInvestmentsAsync();
	Task AddInvestmentAsync(Investment inv);
	Task<List<Conversation>> GetConversationsAsync();
	Task SaveConversationsAsync(List<Conversation> conversations);
	Task<string?> GetSettingAsync(string key);
	Task SaveSettingAsync(string key, string value);
}

public class DataService : IDataService
{
	private readonly ISimulationService _simulation;
	private readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };
	private string _dataRoot = "";

	public string DataRoot => _dataRoot;

	public DataService(ISimulationService simulation)
	{
		_simulation = simulation;
	}

	public async Task InitializeAsync()
	{
		// Se usa el directorio de datos de la app + carpeta GuardacashData (accesible en la mayoría).
		_dataRoot = Path.Combine(FileSystem.AppDataDirectory, PermissionService.DataFolderName);
		Directory.CreateDirectory(_dataRoot);
		await Task.CompletedTask;
	}

	private string PathOf(string file) => Path.Combine(_dataRoot, file);

	private async Task<T> ReadOrSeedAsync<T>(string file, T seed)
	{
		var path = PathOf(file);
		if (File.Exists(path))
		{
			var json = await File.ReadAllTextAsync(path);
			try { return JsonSerializer.Deserialize<T>(json, _jsonOptions) ?? seed; }
			catch { return seed; }
		}
		await SaveAsync(file, seed);
		return seed;
	}

	private async Task SaveAsync<T>(string file, T value)
	{
		Directory.CreateDirectory(_dataRoot);
		var json = JsonSerializer.Serialize(value, _jsonOptions);
		await File.WriteAllTextAsync(PathOf(file), json);
	}

	public async Task<User?> GetProfileAsync() =>
		await ReadOrSeedAsync("perfil.json", new User
		{
			Name = "María González",
			Email = "maria@correo.com",
			Phone = "+505 8888 8888",
			Role = UserRole.Emprendedor,
			City = "Estelí",
			Balance = 18430.50m
		});

	public Task SaveProfileAsync(User user) => SaveAsync("perfil.json", user);

	public Task<List<Service>> GetServicesAsync() =>
		Task.FromResult(_simulation.GetCatalog());

	public async Task<List<Gain>> GetGainsAsync()
	{
		var seed = new List<Gain>
		{
			new Gain { Date = DateTime.Now.AddDays(-1), Description="Venta · Tazas personalizadas", Amount=1120, Cost=640 },
			new Gain { Date = DateTime.Now.AddDays(-3), Description="Cotización aprobada · Diseño logo", Amount=2500, Cost=1100 },
			new Gain { Date = DateTime.Now.AddDays(-5), Description="Sesión fotográfica", Amount=1800, Cost=700 },
		};
		return await ReadOrSeedAsync("ganancias.json", seed);
	}

	public async Task AddGainAsync(Gain gain)
	{
		var list = await GetGainsAsync();
		list.Insert(0, gain);
		await SaveAsync("ganancias.json", list);
	}

	public Task<List<Quote>> GetQuotesAsync() => ReadOrSeedAsync("cotizaciones.json", new List<Quote>());

	public async Task AddQuoteAsync(Quote quote)
	{
		var list = await GetQuotesAsync();
		list.Insert(0, quote);
		await SaveAsync("cotizaciones.json", list);
	}

	public async Task<List<Order>> GetOrdersAsync()
	{
		var seed = new List<Order>
		{
			new Order { ClientName="María", ServiceName="Tazas personalizadas", Status="Entregado", Amount=1120 },
			new Order { ClientName="Carlos", ServiceName="Diseño de logo", Status="En proceso", Amount=2500 },
			new Order { ClientName="Ana", ServiceName="Sesión fotográfica", Status="Pendiente", Amount=1800 },
		};
		return await ReadOrSeedAsync("pedidos.json", seed);
	}

	public async Task AddOrderAsync(Order order)
	{
		var list = await GetOrdersAsync();
		list.Insert(0, order);
		await SaveAsync("pedidos.json", list);
	}

	public async Task<List<Investment>> GetInvestmentsAsync()
	{
		var seed = new List<Investment>
		{
			new Investment { Label="Materiales · Reabastecer inventario", Amount=3000, ProjectedReturn=0.20m },
			new Investment { Label="Marketing · Campaña redes", Amount=1500, ProjectedReturn=0.25m },
			new Investment { Label="Equipamiento · Micrófono", Amount=2500, ProjectedReturn=0.15m },
		};
		return await ReadOrSeedAsync("inversiones.json", seed);
	}

	public async Task AddInvestmentAsync(Investment inv)
	{
		var list = await GetInvestmentsAsync();
		list.Insert(0, inv);
		await SaveAsync("inversiones.json", list);
	}

	public async Task<List<Conversation>> GetConversationsAsync() =>
		await ReadOrSeedAsync("chats.json", _simulation.SeedConversations());

	public Task SaveConversationsAsync(List<Conversation> conversations) =>
		SaveAsync("chats.json", conversations);

	public async Task<string?> GetSettingAsync(string key)
	{
		var dict = await ReadOrSeedAsync("config.json", new Dictionary<string, string>());
		return dict.TryGetValue(key, out var v) ? v : null;
	}

	public async Task SaveSettingAsync(string key, string value)
	{
		var dict = await ReadOrSeedAsync("config.json", new Dictionary<string, string>());
		dict[key] = value;
		await SaveAsync("config.json", dict);
	}
}
