using GuardacashMarket.Services;
using GuardacashMarket.Views;

namespace GuardacashMarket;

public partial class App : Application
{
	public static IServiceProvider Services { get; internal set; } = default!;
	public static Models.Service? CurrentService { get; set; }
	public static Models.Conversation? CurrentConversation { get; set; }

	private readonly IPermissionService _permissionService;

	public App(IPermissionService permissionService)
	{
		InitializeComponent();
		_permissionService = permissionService;
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		Shell shell;
		try
		{
			shell = Services.GetService<AppShell>() ?? new AppShell();
		}
		catch
		{
			shell = new AppShell();
		}

		// Solicita el permiso de almacenamiento al arrancar.
		Dispatcher.Dispatch(async () =>
		{
			await _permissionService.EnsureStoragePermissionAsync(shell);
		});

		return new Window(shell);
	}
}
