using GuardacashMarket.Models;

namespace GuardacashMarket.Services;

public interface ISimulationService
{
	List<Service> GetCatalog();
	List<Conversation> SeedConversations();
	decimal GenerateRandomAmount(decimal min, decimal max);
}

public class SimulationService : ISimulationService
{
	public List<Service> GetCatalog() => new()
	{
		new Service { Name="Sesión fotográfica", OwnerName="Luna Nica", Category="Foto", Price=350, Change=4.9m, Rating=4.9, Reviews=42, IsVerified=true, Description="Sesión de fotos profesional para producto o perfil. Incluye edición básica." },
		new Service { Name="Diseño de logo", OwnerName="Digi Creativa", Category="Diseño", Price=500, Change=6.2m, Rating=4.8, Reviews=31, IsVerified=true, Description="Diseño de identidad visual y logo vectorial con 3 propuestas." },
		new Service { Name="Edición de video", OwnerName="Pixel Norte", Category="Edición", Price=700, Change=2.1m, Rating=4.7, Reviews=27, IsVerified=true, Description="Edición de video para redes, reels y YouTube. Entrega en 48h." },
		new Service { Name="Pulseras artesanales", OwnerName="Barro Vivo", Category="Manualidades", Price=120, Change=8.4m, Rating=4.9, Reviews=64, IsVerified=false, Description="Pulseras tejidas a mano con materiales locales y diseños personalizados." },
		new Service { Name="Reparación de celulares", OwnerName="Tech Estelí", Category="Reparaciones", Price=450, Change=-1.5m, Rating=4.6, Reviews=19, IsVerified=true, Description="Reparación de pantallas, baterías y diagnóstico técnico." },
		new Service { Name="Clase de guitarra", OwnerName="Ritmo Vivo", Category="Música", Price=250, Change=3.3m, Rating=5.0, Reviews=12, IsVerified=false, Description="Clases de guitarra acústica nivel principiante e intermedio." },
		new Service { Name="Maquillaje para eventos", OwnerName="Bella Nica", Category="Maquillaje", Price=400, Change=5.7m, Rating=4.8, Reviews=38, IsVerified=true, Description="Maquillaje profesional para bodas, quinceaños y eventos." },
		new Service { Name="Desarrollo web básico", OwnerName="CodeNica", Category="Tecnología", Price=1200, Change=11.2m, Rating=4.7, Reviews=15, IsVerified=true, Description="Páginas web y landing pages responsive con mantenimiento." },
		new Service { Name="Tazas personalizadas", OwnerName="Barro Vivo", Category="Manualidades", Price=280, Change=2.9m, Rating=4.8, Reviews=51, IsVerified=false, Description="Tazas de cerámica con diseño personalizado a pedido." },
		new Service { Name="Fotografía de producto", OwnerName="Luna Nica", Category="Foto", Price=600, Change=1.8m, Rating=4.9, Reviews=22, IsVerified=true, Description="Fotografía comercial para catálogos y tiendas online." },
	};

	public List<Conversation> SeedConversations() => new()
	{
		new Conversation { OtherId="u-luna", OtherName="Luna Nica", OtherInitials="LN" },
		new Conversation { OtherId="u-barro", OtherName="Barro Vivo", OtherInitials="BV" },
		new Conversation { OtherId="u-digi", OtherName="Digi Creativa", OtherInitials="DC" },
		new Conversation { OtherId="u-tech", OtherName="Tech Estelí", OtherInitials="TE" },
	};

	public decimal GenerateRandomAmount(decimal min, decimal max)
	{
		var rand = new Random();
		return Math.Round(min + (decimal)rand.NextDouble() * (max - min), 2);
	}
}
