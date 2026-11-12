using GuardacashMarket.Models;

namespace GuardacashMarket.Services;

public interface IChatBotService
{
	string GenerateReply(string contactName, string userMessage);
}

public class ChatBotService : IChatBotService
{
	public string GenerateReply(string contactName, string userMessage)
	{
		var msg = userMessage.ToLowerInvariant();

		if (ContainsAny(msg, "hola", "buenas", "saludos", "hey", "buenos días", "buenas tardes"))
			return $"{contactName}: ¡Hola! 👋 Gracias por contactarme. ¿En qué puedo ayudarte con mis servicios?";

		if (ContainsAny(msg, "precio", "cuánto", "costo", "cuanto", "valor", "tarifa"))
			return $"{contactName}: El precio depende del servicio. Para un pedido estándar manejo una tarifa de C$ 300–1200. Cuéntame tu necesidad y te doy una cotización exacta. 📋";

		if (ContainsAny(msg, "cotiza", "cotización", "presupuesto"))
			return $"{contactName}: ¡Claro! Puedo prepararte una cotización detallada. Necesito saber el tipo de servicio y la cantidad. ¿Me pasas más detalles? ✍️";

		if (ContainsAny(msg, "disponible", "disponibilidad", "fecha", "cuándo", "cuando", "agenda"))
			return $"{contactName}: Sí, tengo disponibilidad esta semana. Puedo agendarte entre lunes y sábado. ¿Qué día te queda mejor? 📅";

		if (ContainsAny(msg, "envío", "envio", "entrega", "domicilio"))
			return $"{contactName}: Hago entregas en la zona de Estelí sin costo extra, y envíos al resto del país con un costo pequeño. ¿A qué zona necesitas? 📦";

		if (ContainsAny(msg, "diseño", "personaliz", "a medida"))
			return $"{contactName}: Sí, puedo personalizar el servicio según tus gustos. Cuéntame más sobre lo que imaginas y le damos ese toque único. 🎨";

		if (ContainsAny(msg, "calidad", "garantía", "seguro", "confianza"))
			return $"{contactName}: En GUARDACASH MARKET priorizamos la confianza. Cuento con respaldo y valoraciones de clientes reales, y puedo mostrarte ejemplos de mi trabajo. ✅";

		if (ContainsAny(msg, "gracias", "cambia", "ok", "perfecto", "listo"))
			return $"{contactName}: ¡Con gusto! 😊 Quedo atento a cualquier otra consulta. ¡Que tengas un excelente día!";

		if (ContainsAny(msg, "servicio", "ofreces", "haces", "trabajo"))
			return $"{contactName}: Me dedico a {DescribeOffer(contactName)}. Puedo ajustar el alcance a tu presupuesto y necesidad. ¿Te interesa? 💼";

		return $"{contactName}: Entiendo. ¿Podrías darme un poco más de detalle para ayudarte mejor? Reviso tu consulta sobre: \"{userMessage}\". 🤖";
	}

	private static bool ContainsAny(string text, params string[] tokens)
		=> tokens.Any(t => text.Contains(t));

	private static string DescribeOffer(string name) => name switch
	{
		"Luna Nica" => "fotografía y sesiones de producto",
		"Barro Vivo" => "manualidades y artesanías personalizadas",
		"Digi Creativa" => "diseño gráfico y logos",
		"Tech Estelí" => "reparación de equipos tecnológicos",
		_ => "servicios de calidad para tu negocio"
	};
}
