namespace MicroServicioLogin.Application.Interfaces
{
    public interface IAuthService
    {
        bool ValidateUser(string username, string password);
        void RegisterUser(string username, string password, string email);
        string GenerateToken(string username);
    }
}
