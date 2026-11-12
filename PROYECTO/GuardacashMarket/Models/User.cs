namespace GuardacashMarket.Models;

public enum UserRole
{
	Emprendedor,
	Cliente
}

public class User
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string Name { get; set; } = "";
	public string Email { get; set; } = "";
	public string Phone { get; set; } = "";
	public UserRole Role { get; set; } = UserRole.Cliente;
	public bool IsVerified { get; set; }
	public string City { get; set; } = "Estelí";
	public decimal Balance { get; set; }
}
