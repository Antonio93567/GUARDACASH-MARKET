using GuardacashMarket.Services;
using GuardacashMarket.Views;
using Microsoft.Extensions.Logging;

namespace GuardacashMarket;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("Poppins-Regular.ttf", "Poppins");
				fonts.AddFont("Poppins-Medium.ttf", "PoppinsMedium");
				fonts.AddFont("Poppins-SemiBold.ttf", "PoppinsSemiBold");
				fonts.AddFont("Poppins-Bold.ttf", "PoppinsBold");
				fonts.AddFont("Montserrat.ttf", "Montserrat");
			});

		builder.Services.AddSingleton<IDataService, DataService>();
		builder.Services.AddSingleton<ISimulationService, SimulationService>();
		builder.Services.AddSingleton<IChatBotService, ChatBotService>();
		builder.Services.AddSingleton<IPermissionService, PermissionService>();

		builder.Services.AddSingleton<SplashPage>();
		builder.Services.AddSingleton<OnboardingPage>();
		builder.Services.AddSingleton<LoginPage>();
		builder.Services.AddSingleton<RegisterPage>();
		builder.Services.AddSingleton<DashboardPage>();
		builder.Services.AddSingleton<MarketPage>();
		builder.Services.AddSingleton<ServiceDetailPage>();
		builder.Services.AddSingleton<CopilotPage>();
		builder.Services.AddSingleton<MessagesPage>();
		builder.Services.AddSingleton<ChatPage>();
		builder.Services.AddSingleton<ProfilePage>();
		builder.Services.AddSingleton<SettingsPage>();
		builder.Services.AddSingleton<InvestPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		var app = builder.Build();
		App.Services = app.Services;
		return app;
	}
}
