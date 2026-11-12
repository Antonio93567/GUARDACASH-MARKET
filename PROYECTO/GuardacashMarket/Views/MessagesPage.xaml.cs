using GuardacashMarket.Models;
using GuardacashMarket.Services;

namespace GuardacashMarket.Views;

public partial class MessagesPage : ContentPage
{
	private readonly IDataService _data;
	private readonly ISimulationService _sim;
	private List<Conversation> _conversations = new();

	public MessagesPage(IDataService data, ISimulationService sim)
	{
		InitializeComponent();
		_data = data;
		_sim = sim;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		_conversations = await _data.GetConversationsAsync();
		RefreshList();
	}

	private void RefreshList()
	{
		var sorted = _conversations.OrderByDescending(c =>
			c.Messages.Count > 0 ? c.Messages[^1].Timestamp : DateTime.MinValue).ToList();
		ConvosView.ItemsSource = sorted;
	}

	private async void OnConvoSelected(object? sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection.FirstOrDefault() is Conversation convo)
		{
			App.CurrentConversation = convo;
			ConvosView.SelectedItem = null;
			await Shell.Current.GoToAsync("//Chat");
		}
	}

	private async void OnDashboardClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Dashboard");
	private async void OnMarketClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Market");
	private async void OnCopilotClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Copilot");
	private async void OnProfileClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Profile");
}
