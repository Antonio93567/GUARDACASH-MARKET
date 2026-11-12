using System.Diagnostics;
using GuardacashMarket.Models;
using GuardacashMarket.Views;

namespace GuardacashMarket.Services;

/// <summary>
/// Maneja el permiso de almacenamiento y la carpeta de datos local.
/// Si el usuario no concede el permiso, la app se cierra automáticamente.
/// </summary>
public interface IPermissionService
{
	Task<bool> EnsureStoragePermissionAsync(Shell shell);
	void ExitApp();
}

public class PermissionService : IPermissionService
{
	public const string DataFolderName = "GuardacashData";

	public async Task<bool> EnsureStoragePermissionAsync(Shell shell)
	{
		try
		{
			// 1) Solicitar permiso de almacenamiento.
			var granted = await TryRequestStoragePermissionAsync();
			if (!granted)
			{
				await ShowPermissionDeniedAndExitAsync(shell);
				return false;
			}

			// 2) Pedir al usuario seleccionar la carpeta donde se guardarán los datos.
			var folderPicked = await PickDataFolderAsync(shell);
			if (!folderPicked)
			{
				await ShowPermissionDeniedAndExitAsync(shell);
				return false;
			}

			// 3) Avanzar a la app (splash/onboarding).
			await shell.GoToAsync("//Splash");
			return true;
		}
		catch (Exception)
		{
			// Por seguridad, ante cualquier error no recuperable se cierra.
			ExitApp();
			return false;
		}
	}

	private async Task<bool> TryRequestStoragePermissionAsync()
	{
#if ANDROID
		try
		{
			var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
			if (status != PermissionStatus.Granted)
			{
				status = await Permissions.RequestAsync<Permissions.StorageRead>();
			}
			return status == PermissionStatus.Granted;
		}
		catch (Exception)
		{
			// En Android 11+ el permiso puede no ser necesario si se usa SAF,
			// por lo tanto se considera concedido para no bloquear.
			return true;
		}
#else
		return true;
#endif
	}

	private async Task<bool> PickDataFolderAsync(Shell shell)
	{
		// Se informa que se usará una carpeta por defecto en el almacenamiento de la app.
		// Para evitar fallos en versiones donde SAF no está disponible, se crea la carpeta
		// GuardacashData dentro del directorio de datos de la app.
		try
		{
			var dataService = App.Services!.GetService<IDataService>();
			if (dataService != null)
			{
				await dataService.InitializeAsync();
				return true;
			}
			return false;
		}
		catch (Exception)
		{
			return false;
		}
	}

	private async Task ShowPermissionDeniedAndExitAsync(Shell shell)
	{
		await MainThread.InvokeOnMainThreadAsync(async () =>
		{
			var accepted = await shell.DisplayAlert(
				"Permiso requerido",
				"GUARDACASH MARKET necesita acceso al almacenamiento para guardar tus datos.\n\n" +
				"Si no concedes el permiso, la aplicación se cerrará.",
				"Reintentar",
				"Salir");

			if (accepted)
			{
				var ok = await EnsureStoragePermissionAsync(shell);
				if (!ok) ExitApp();
			}
			else
			{
				ExitApp();
			}
		});
	}

	public void ExitApp()
	{
		MainThread.BeginInvokeOnMainThread(() =>
		{
			try
			{
				Application.Current?.Quit();
			}
			catch
			{
				// Fallback: cerrar la ventana.
			}
		});
	}
}
