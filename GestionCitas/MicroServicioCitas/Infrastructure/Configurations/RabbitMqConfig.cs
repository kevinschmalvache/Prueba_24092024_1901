using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroServicioCitas.Infrastructure.Configurations
{
    public class RabbitMqConfig
    {
        public string HostName { get; set; } = "localhost";
        public int Port { get; set; } = 5672;
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
    }
}