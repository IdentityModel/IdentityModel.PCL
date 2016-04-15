using System;

namespace IdentityModel.Client
{
    public class TokenRefreshEventArgs : EventArgs
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public bool IsError { get; set; }
        public string Error { get; set; }
    }
}