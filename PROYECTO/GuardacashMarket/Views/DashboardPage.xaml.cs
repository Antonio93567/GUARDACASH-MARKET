using GuardacashMarket.Models;
using GuardacashMarket.Services;

namespace GuardacashMarket.Views;

public partial class DashboardPage : ContentPage
{
	private readonly IDataService _data;

	public DashboardPage(IDataService data)
	{
		InitializeComponent();
		_data = data;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		try
		{
			var profile = await _data.GetProfileAsync();
			if (profile != null)
			{
				var nombre = profile.Name.Split(' ')[0];
				NameLabel.Text = nombre + " " + (profile.Name.Contains(' ') ? profile.Name.Split(' ')[^1].Substring(0,1) + "." : "");
				BalanceLabel.Text = $"C$ {profile.Balance:N0}";
				GreetingLabel.Text = EsManana() ? "Buenos días," : "Buenas tardes,";
			}

			var gains = await _data.GetGainsAsync();
			GainsView.ItemsSource = gains;
		}
		catch { }
	}

	private static bool EsManana() => DateTime.Now.Hour < 12;

	private async void OnCotizarClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Copilot");
	private async void OnGananciaClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Copilot");
	private async void OnInvestClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Invest");
	private async void OnPromoClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Market");
	private async void OnNotifyClicked(object? sender, EventArgs e) => await DisplayAlert("Notificaciones", "Tienes 3 nuevas cotizaciones por revisar.", "Ok");

	private async void OnTabClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Dashboard");
	private async void OnMarketClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Market");
	private async void OnCopilotClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Copilot");
	private async void OnMessagesClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Messages");
	private async void OnProfileClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Profile");
}
