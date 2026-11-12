using GuardacashMarket.Models;
using GuardacashMarket.Services;
using Microsoft.Maui.Graphics;

namespace GuardacashMarket.Views;

public partial class ServiceDetailPage : ContentPage
{
	private readonly IDataService _data;
	private Service? _service;
	private readonly Random _rand = new();

	public ServiceDetailPage(IDataService data)
	{
		InitializeComponent();
		_data = data;
		Chart.Drawable = new LineChartDrawable();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		_service = App.CurrentService;
		if (_service == null) return;

		TitleLabel.Text = _service.Name;
		SvName.Text = _service.Name;
		SvOwner.Text = _service.OwnerName + " · " + _service.Category;
		SvPrice.Text = $"C$ {_service.Price:N0}";
		SvRating.Text = $"★ {_service.Rating:N1}  ({_service.Reviews} reseñas)";
		SvDesc.Text = _service.Description;
		SvIcon.Text = _service.Icon;
		_regenerateLine("3M");
		Chart.Invalidate();
	}

	private async void OnBackClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Market");

	private async void OnBuyClicked(object? sender, EventArgs e)
	{
		if (_service != null)
		{
			var order = new Order
			{
				ClientName = "Tú",
				ServiceName = _service.Name,
				Status = "Pendiente",
				Amount = _service.Price
			};
			await _data.AddOrderAsync(order);
		}
		await DisplayAlert("Pedido", $"Solicitaste: '{_service?.Name}'. El vendedor será notificado.", "Ok");
	}

	private async void OnSellClicked(object? sender, EventArgs e)
	{
		// Contacta con el emprendedor (bot conversacional por perfil).
		if (_service != null)
		{
			var convos = await _data.GetConversationsAsync();
			var convo = convos.FirstOrDefault(c => c.OtherName == _service.OwnerName)
				?? new Conversation { OtherName = _service.OwnerName, OtherInitials = Initials(_service.OwnerName) };
			App.CurrentConversation = convo;
		}
		await Shell.Current.GoToAsync("//Chat");
	}

	private static string Initials(string name)
	{
		var parts = name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
		return parts.Length >= 2 ? parts[0][0].ToString() + parts[1][0] : name.Substring(0, Math.Min(2, name.Length)).ToUpper();
	}

	private void OnFilterClicked(object? sender, EventArgs e)
	{
		if (sender is Button btn)
		{
			_regenerateLine(btn.Text ?? "3M");
			Chart.Invalidate();
			try { DisplayAlert("Filtro", $"Mostrando tendencia {btn.Text}.", "Ok"); } catch { }
		}
	}

	private void _regenerateLine(string period)
	{
		var points = new List<float>();
		var n = 40;
		for (int i = 0; i < n; i++)
		{
			var trend = period switch { "1Y" => 0.02f, "1M" => 0.05f, "1D" => 0.0f, _ => 0.03f };
			var baseVal = 0.5f + i * trend;
			var noise = (float)(_rand.NextDouble() - 0.5) * 0.08f;
			points.Add(Math.Clamp(baseVal + noise, 0.1f, 0.95f));
		}
		if (Chart.Drawable is LineChartDrawable d)
			d.Points = points;
	}
}

public class LineChartDrawable : IDrawable
{
	public List<float> Points { get; set; } = new() { 0.3f, 0.35f, 0.32f, 0.4f, 0.45f, 0.42f, 0.5f, 0.55f, 0.6f, 0.58f, 0.65f, 0.7f };

	public void Draw(ICanvas canvas, RectF dirtyRect)
	{
		canvas.FillColor = Color.FromArgb("#0E1418");
		canvas.FillRectangle(dirtyRect);
		canvas.StrokeColor = Color.FromArgb("#00FFFF");
		canvas.StrokeSize = 2;

		var w = dirtyRect.Width;
		var h = dirtyRect.Height;
		if (Points.Count < 2) return;

		var path = new PathF();
		for (int i = 0; i < Points.Count; i++)
		{
			var x = (float)i / (Points.Count - 1) * w;
			var y = h - Points[i] * h;
			if (i == 0) path.MoveTo(x, y);
			else path.LineTo(x, y);
		}
		canvas.DrawPath(path);

		// Área bajo la línea
		var area = new PathF();
		area.MoveTo(0, h);
		for (int i = 0; i < Points.Count; i++)
		{
			var x = (float)i / (Points.Count - 1) * w;
			var y = h - Points[i] * h;
			area.LineTo(x, y);
		}
		area.LineTo(w, h);
		area.Close();
		canvas.FillColor = Color.FromArgb("#3300FFFF");
		canvas.FillPath(area);
	}
}
