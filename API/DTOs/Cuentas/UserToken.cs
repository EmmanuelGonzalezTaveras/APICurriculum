using NuGet.Common;

namespace API.DTOs.Cuentas
{
    public class UserToken
    {
        public string Token { get; set; }
        public DateTime Expiracion { get; set; }
    }
}
