namespace MicroServicioRecetas.Infrastructure.Configurations
{
    public class RabbitMqConfig
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public RabbitMqConfig()
        {
            // Configuración por defecto
            HostName = "localhost"; // Cambia esto si tu RabbitMQ está en otro host
            Port = 5672; // Puerto por defecto de RabbitMQ
            UserName = "guest"; // Usuario por defecto
            Password = "guest"; // Contraseña por defecto
        }
    }
}
