using GuardacashMarket.Models;
using GuardacashMarket.Services;

namespace GuardacashMarket.Views;

public partial class ProfilePage : ContentPage
{
	private readonly IDataService _data;

	public ProfilePage(IDataService data)
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
				ProfileName.Text = profile.Name;
				var initials = GetInitials(profile.Name);
				AvatarLabel.Text = initials;
				ProfileRole.Text = (profile.Role == UserRole.Emprendedor ? "Emprendedor" : "Cliente") + " · " + profile.City;
				ProfileBalance.Text = $"C$ {profile.Balance:N0}";
			}
		}
		catch { }
	}

	private static string GetInitials(string name)
	{
		var parts = name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
		if (parts.Length >= 2) return parts[0][0].ToString() + parts[1][0];
		return name.Length > 0 ? name.Substring(0, Math.Min(2, name.Length)).ToUpper() : "GC";
	}

	private async void OnOrdersClicked(object? sender, EventArgs e)
	{
		var orders = await _data.GetOrdersAsync();
		var text = string.Join("\n", orders.Select(o => $"• {o.ServiceName} — C$ {o.Amount:N0} ({o.Status})"));
		await DisplayAlert("Mis pedidos", string.IsNullOrEmpty(text) ? "Sin pedidos." : text, "Ok");
	}

	private async void OnSavedClicked(object? sender, EventArgs e) => await DisplayAlert("Guardados", "Aún no has guardado servicios.", "Ok");
	private async void OnSellerClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Copilot");
	private async void OnSettingsClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Settings");
	private async void OnLogoutClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Login");

	private async void OnDashboardClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Dashboard");
	private async void OnMarketClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Market");
	private async void OnCopilotClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Copilot");
	private async void OnMessagesClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Messages");
}
