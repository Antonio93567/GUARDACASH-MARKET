using GuardacashMarket.Services;

namespace GuardacashMarket.Views;

public partial class SettingsPage : ContentPage
{
	private readonly IDataService _data;
	private readonly IPermissionService _permissions;

	public SettingsPage(IDataService data, IPermissionService permissions)
	{
		InitializeComponent();
		_data = data;
		_permissions = permissions;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		FolderLabel.Text = string.IsNullOrEmpty(_data.DataRoot)
			? "No configurada" : _data.DataRoot;
	}

	private async void OnChangeFolderClicked(object? sender, EventArgs e)
	{
		await DisplayAlert("Carpeta de datos",
			"Los datos se guardan en la carpeta GuardacashData dentro del almacenamiento de la aplicación en el dispositivo.\n\n" +
			"(En esta versión la app usa su almacenamiento privado, accesible sin permisos adicionales.)",
			"Ok");
	}

	private async void OnBackClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Profile");
}
