using GuardacashMarket.Models;
using GuardacashMarket.Services;

namespace GuardacashMarket.Views;

public partial class CopilotPage : ContentPage
{
	private readonly IDataService _data;

	public CopilotPage(IDataService data)
	{
		InitializeComponent();
		_data = data;
	}

	private async void OnCalcClicked(object? sender, EventArgs e)
	{
		decimal.TryParse(MaterialEntry.Text, out var material);
		decimal.TryParse(LaborEntry.Text, out var labor);

		var total = material + labor;
		TotalLabel.Text = $"C$ {total:N0}";
		SuggestedLabel.Text = $"C$ {Math.Round(total * 1.25m, 0):N0}";

		if (!string.IsNullOrWhiteSpace(ClientEntry.Text) && !string.IsNullOrWhiteSpace(ServiceEntry.Text))
		{
			await _data.AddQuoteAsync(new Quote
			{
				ClientName = ClientEntry.Text,
				ServiceName = ServiceEntry.Text,
				MaterialCost = material,
				LaborCost = labor
			});
		}

		await DisplayAlert("Cotización", $"Costo total: C$ {total:N0}\nPrecio sugerido (margen 25%): C$ {Math.Round(total * 1.25m,0):N0}", "Ok");
	}

	private async void OnGainClicked(object? sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(GainDescEntry.Text) ||
			string.IsNullOrWhiteSpace(GainAmountEntry.Text))
		{
			await DisplayAlert("Aviso", "Completa la descripción y el ingreso.", "Ok");
			return;
		}

		decimal.TryParse(GainAmountEntry.Text, out var amount);
		decimal.TryParse(GainCostEntry.Text, out var cost);

		await _data.AddGainAsync(new Gain
		{
			Description = GainDescEntry.Text,
			Amount = amount,
			Cost = cost
		});

		GainDescEntry.Text = "";
		GainAmountEntry.Text = "";
		GainCostEntry.Text = "";
		await DisplayAlert("Listo", "Ganancia registrada con éxito.", "Ok");
	}

	private async void OnDashboardClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Dashboard");
	private async void OnMarketClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Market");
	private async void OnMessagesClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Messages");
	private async void OnProfileClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Profile");
}
