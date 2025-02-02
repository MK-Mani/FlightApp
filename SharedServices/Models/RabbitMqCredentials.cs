﻿namespace SharedServices.Models
{
    public class RabbitMqCredentials
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string HostName { get; set; }

        public string VHost { get; set; } = "/";

        public int Port { get; set;  }
    }
}
