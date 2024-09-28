namespace MicroServicioCitas.Infrastructure.Configurations
{
    public class RabbitMqConfig
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public RabbitMqConfig()
        {
            HostName = "localhost";
            Port = 5672;
            UserName = "guest"; // Usuario por defecto
            Password = "guest"; // Contraseña por defecto
        }
    }
}