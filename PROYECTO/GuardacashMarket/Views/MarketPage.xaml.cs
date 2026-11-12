using GuardacashMarket.Models;
using GuardacashMarket.Services;

namespace GuardacashMarket.Views;

public partial class MarketPage : ContentPage
{
	private readonly IDataService _data;
	private IReadOnlyList<Service> _allServices = Array.Empty<Service>();
	private Service? _selected;

	public MarketPage(IDataService data)
	{
		InitializeComponent();
		_data = data;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		_allServices = await _data.GetServicesAsync();
		ServicesView.ItemsSource = _allServices;
		Search.TextChanged += (s, e) => ApplyFilter();
	}

	private void ApplyFilter()
	{
		var q = Search?.Text?.Trim().ToLowerInvariant() ?? "";
		if (string.IsNullOrEmpty(q))
			ServicesView.ItemsSource = _allServices;
		else
			ServicesView.ItemsSource = _allServices.Where(x =>
				x.Name.ToLowerInvariant().Contains(q) ||
				x.OwnerName.ToLowerInvariant().Contains(q) ||
				x.Category.ToLowerInvariant().Contains(q)).ToList();
	}

	private async void OnServiceSelected(object? sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection.FirstOrDefault() is Service service)
		{
			_selected = service;
			App.CurrentService = service;
			await Shell.Current.GoToAsync($"//Detail?serviceId={service.Id}");
		}
		ServicesView.SelectedItem = null;
	}

	private async void OnDashboardClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Dashboard");
	private async void OnCopilotClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Copilot");
	private async void OnMessagesClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Messages");
	private async void OnProfileClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Profile");
}
