using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicProMvcWebApp.Models
{
    public class Usuario : BaseEntityData
    {
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<Rol> Roles { get; set; } = new HashSet<Rol>();
        public ICollection<RefreshToken> RefreshTokens{ get; set; } = new HashSet<RefreshToken>();

        public ICollection<UsuariosRoles> UsuariosRoles { get; set; }
    }
}
