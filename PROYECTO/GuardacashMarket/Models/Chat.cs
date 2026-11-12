using System;

namespace GuardacashMarket.Models;

public class ChatMessage
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string ConversationId { get; set; } = "";
	public string SenderName { get; set; } = "";
	public bool IsUser { get; set; }
	public string Text { get; set; } = "";
	public DateTime Timestamp { get; set; } = DateTime.Now;
}

public class Conversation
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string OtherId { get; set; } = "";
	public string OtherName { get; set; } = "";
	public string OtherInitials { get; set; } = "";
	public bool IsBot { get; set; } = true;
	public List<ChatMessage> Messages { get; set; } = new();
	public string LastMessage => Messages.Count > 0 ? Messages[^1].Text : "";
}
