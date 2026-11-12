using GuardacashMarket.Services;

namespace GuardacashMarket.Views;

public partial class LoginPage : ContentPage
{
	private readonly IDataService _data;

	public LoginPage(IDataService data)
	{
		InitializeComponent();
		_data = data;
	}

	private async void OnLoginClicked(object? sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(EmailEntry.Text))
		{
			await DisplayAlert("Aviso", "Ingresa tu correo electrónico.", "Ok");
			return;
		}
		await Shell.Current.GoToAsync("//Dashboard");
	}

	private async void OnRegisterClicked(object? sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//Register");
	}
}
