using GuardacashMarket.Models;
using GuardacashMarket.Services;

namespace GuardacashMarket.Views;

public partial class ChatPage : ContentPage
{
	private readonly IDataService _data;
	private readonly IChatBotService _bot;
	private readonly ISimulationService _sim;
	private Conversation? _convo;
	private string _userName = "Tú";

	public ChatPage(IDataService data, IChatBotService bot, ISimulationService sim)
	{
		InitializeComponent();
		_data = data;
		_bot = bot;
		_sim = sim;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		try
		{
			var profile = await _data.GetProfileAsync();
			if (profile != null) _userName = profile.Name.Split(' ')[0];

			var convos = await _data.GetConversationsAsync();
			_convo = App.CurrentConversation;
			if (_convo == null)
			{
				_convo = convos.FirstOrDefault();
			}
			else
			{
				// Reconciliar con la lista persistida.
				var existing = convos.FirstOrDefault(c => c.Id == _convo.Id);
				_convo = existing ?? _convo;
			}

			if (_convo == null)
			{
				_convo = new Conversation { OtherName = "Asistente", OtherInitials = "AS" };
			}

			// Mensaje de bienvenida del bot si está vacío.
			if (_convo.Messages.Count == 0)
			{
				_convo.Messages.Add(new ChatMessage
				{
					ConversationId = _convo.Id,
					SenderName = _convo.OtherName,
					IsUser = false,
					Text = $"¡Hola! Soy {_convo.OtherName} 🤖 Soy el asistente virtual de este perfil. ¿En qué puedo ayudarte?",
					Timestamp = DateTime.Now
				});
				await SaveAsync();
			}

			ContactName.Text = _convo.OtherName;
			MsgView.ItemsSource = _convo.Messages.ToList();
			await ScrollToBottom();
		}
		catch { }
	}

	private async void OnSendClicked(object? sender, EventArgs e)
	{
		var text = InputEntry.Text?.Trim();
		if (string.IsNullOrEmpty(text) || _convo == null)
		{
			InputEntry.Text = "";
			return;
		}

		_convo.Messages.Add(new ChatMessage
		{
			ConversationId = _convo.Id,
			SenderName = _userName,
			IsUser = true,
			Text = text,
			Timestamp = DateTime.Now
		});

		// Respuesta del bot simulada (pequeña pausa).
		InputEntry.Text = "";
		Refresh();

		await Task.Delay(700);
		var reply = _bot.GenerateReply(_convo.OtherName, text);
		var parts = reply.Split(new[] { ": " }, 2, StringSplitOptions.None);
		var replyText = parts.Length == 2 ? parts[1] : reply;

		_convo.Messages.Add(new ChatMessage
		{
			ConversationId = _convo.Id,
			SenderName = _convo.OtherName,
			IsUser = false,
			Text = replyText,
			Timestamp = DateTime.Now
		});

		Refresh();
		await SaveAsync();
		await ScrollToBottom();
	}

	private void Refresh() => MsgView.ItemsSource = _convo!.Messages.ToList();

	private async Task ScrollToBottom()
	{
		if (_convo != null && _convo.Messages.Count > 0)
			await MainThread.InvokeOnMainThreadAsync(() => MsgView.ScrollTo(_convo.Messages[^1], position: ScrollToPosition.End, animate: true));
	}

	private async Task SaveAsync()
	{
		if (_convo == null) return;
		var convos = await _data.GetConversationsAsync();
		var idx = convos.FindIndex(c => c.Id == _convo.Id);
		if (idx >= 0) convos[idx] = _convo;
		else convos.Add(_convo);
		await _data.SaveConversationsAsync(convos);
	}

	private async void OnBackClicked(object? sender, EventArgs e) => await Shell.Current.GoToAsync("//Messages");
}
