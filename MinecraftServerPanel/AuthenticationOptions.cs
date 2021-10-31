using System;

namespace MinecraftServerPanel
{
    public class AuthenticationOptions
    {
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string[] ValidUsers { get; set; } = Array.Empty<string>();
    }
}