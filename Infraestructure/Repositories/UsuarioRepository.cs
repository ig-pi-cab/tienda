using Core.Entities;
using Core.Interfaces;
using Infraestructure.Data;
using Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Infrastructure.Repositories;

public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(TiendaContexto context) : base(context)
    {
        
    }

    public async Task<Usuario> GetByRefreshTokenAsync(string refreshToken)
    {
        return await _context.Usuarios
            .Include(u => u.Roles)
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));
    }

    public async Task<Usuario> GetByUsernameAsync(string username)
    {
        return await _context.Usuarios
            .Include(u=>u.Roles)
            .Include(u=>u.RefreshTokens)
            .FirstOrDefaultAsync(U=>U.Username.ToLower()==username.ToLower());
    }

    
}

