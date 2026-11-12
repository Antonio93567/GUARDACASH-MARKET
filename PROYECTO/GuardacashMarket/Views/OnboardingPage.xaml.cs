namespace GuardacashMarket.Views;

public partial class OnboardingPage : ContentPage
{
	private readonly string[] _titles =
	{
		"Tu idea merece un lugar",
		"Conecta con personas reales",
		"Haz crecer tu emprendimiento"
	};

	private readonly string[] _bodies =
	{
		"Convierte tus habilidades en un negocio real y genera ingresos con GUARDACASH MARKET.",
		"Habla directamente con quienes crean lo que buscas. Pregunta, cotiza y compra con confianza.",
		"Publica tus servicios, gestiona tus ganancias y reinvierte para hacer crecer tu negocio."
	};

	private int _index;

	public OnboardingPage()
	{
		InitializeComponent();
		Render();
	}

	private void Render()
	{
		SlideTitle.Text = _titles[_index];
		SlideBody.Text = _bodies[_index];
		DotLabel.Text = _index switch
		{
			0 => "●  ○  ○",
			1 => "○  ●  ○",
			_ => "○  ○  ●"
		};
		NextBtn.Text = _index == 2 ? "Empezar" : "Continuar";
	}

	private async void OnNextClicked(object? sender, EventArgs e)
	{
		if (_index < 2)
		{
			_index++;
			Render();
		}
		else
		{
			await Shell.Current.GoToAsync("//Register");
		}
	}

	private async void OnLoginTapped(object? sender, TappedEventArgs e)
	{
		await Shell.Current.GoToAsync("//Login");
	}
}
