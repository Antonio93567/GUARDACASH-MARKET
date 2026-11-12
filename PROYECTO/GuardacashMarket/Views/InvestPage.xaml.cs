using GuardacashMarket.Models;
using GuardacashMarket.Services;

namespace GuardacashMarket.Views;

public partial class InvestPage : ContentPage
{
	private readonly IDataService _data;

	public InvestPage(IDataService data)
	{
		InitializeComponent();
		_data = data;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		var invs = await _data.GetInvestmentsAsync();
		InvView.ItemsSource = invs;
	}

	private static decimal ParseReturn(string? s)
	{
		if (string.IsNullOrWhiteSpace(s)) return 0.18m;
		var digits = s.Trim().TrimEnd('%');
		return decimal.TryParse(digits, out var v) ? v / 100m : 0.18m;
	}

	private async void OnInvestClicked(object? sender, EventArgs e)
	{
		decimal.TryParse(AmountEntry.Text, out var amount);
		if (amount <= 0)
		{
			await DisplayAlert("Aviso", "Ingresa un monto válido (C$ > 0).", "Ok");
			return;
		}

		var ret = ParseReturn(ReturnPicker.SelectedItem?.ToString());
		var label = AreaPicker.SelectedItem?.ToString() ?? "Reinversión general";
		var projectedProfit = Math.Round(amount * ret, 2);

		ProjLabel.Text = $"C$ {amount + projectedProfit:N0}";
		GainLabel.Text = $"C$ {projectedProfit:N0}";

		await _data.AddInvestmentAsync(new Investment
		{
			Label = label,
			Amount = amount,
			ProjectedReturn = ret
		});

		var invs = await _data.GetInvestmentsAsync();
		InvView.ItemsSource = invs;

		await DisplayAlert("Inversión simulada",
			$"Invertiste C$ {amount:N0} en: {label}\nRetorno esperado: {ret:P0}\nGanancia estimada: C$ {projectedProfit:N0}",
			"Ok");
	}

	private async void OnBackClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Dashboard");
	private async void OnDashboardClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Dashboard");
}
