using GuardacashMarket.Models;
using GuardacashMarket.Services;

namespace GuardacashMarket.Views;

public partial class RegisterPage : ContentPage
{
	private readonly IDataService _data;
	private UserRole _selectedRole = UserRole.Emprendedor;

	public RegisterPage(IDataService data)
	{
		InitializeComponent();
		_data = data;
	}

	private async void OnRoleClicked(object? sender, EventArgs e)
	{
		if (sender is Button btn)
		{
			bool isSeller = btn == SellerBtn;
			_selectedRole = isSeller ? UserRole.Emprendedor : UserRole.Cliente;
			ApplyRoleStyles();
		}
	}

	private void ApplyRoleStyles()
	{
		bool isSeller = _selectedRole == UserRole.Emprendedor;
		SellerBtn.BackgroundColor = isSeller ? Color.FromArgb("#1E40AF") : Colors.Transparent;
		SellerBtn.TextColor = isSeller ? Colors.White : Color.FromArgb("#00FFFF");
		SellerBtn.BorderColor = Color.FromArgb("#00FFFF");
		BuyerBtn.BackgroundColor = isSeller ? Colors.Transparent : Color.FromArgb("#1E40AF");
		BuyerBtn.TextColor = isSeller ? Color.FromArgb("#00FFFF") : Colors.White;
		BuyerBtn.BorderColor = Color.FromArgb("#00FFFF");
	}

	private async void OnContinueClicked(object? sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(NameEntry.Text) ||
			string.IsNullOrWhiteSpace(EmailEntry.Text) ||
			string.IsNullOrWhiteSpace(PhoneEntry.Text))
		{
			await DisplayAlert("Aviso", "Completa todos los campos para continuar.", "Ok");
			return;
		}

		var profile = await _data.GetProfileAsync() ?? new User();
		profile.Name = NameEntry.Text;
		profile.Email = EmailEntry.Text;
		profile.Phone = PhoneEntry.Text;
		profile.Role = _selectedRole;
		await _data.SaveProfileAsync(profile);

		await Shell.Current.GoToAsync("//Dashboard");
	}

	private async void OnBackClicked(object? sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//Onboarding");
	}
}
