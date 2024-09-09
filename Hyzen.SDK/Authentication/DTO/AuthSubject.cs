using System.Security.Claims;

namespace Hyzen.SDK.Authentication.DTO;

public class AuthSubject
{
    public Guid Guid { get; set; } 
    public string Name { get; set; }
    public string Email { get; set; }
    public List<string> Groups { get; set; } = [];
    public List<string> Roles { get; set; } = [];
    
    public bool HasRole(string roleKey)
    {
        return Roles.Contains(roleKey);
    }
        
    public bool HasGroup(string groupKey)
    {
        return Groups.Contains(groupKey);
    }
    
    public Claim[] ToClaims()
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, Guid.ToString()),
            new(ClaimTypes.Name, Name),
            new(ClaimTypes.Email, Email)
        };
        
        claims.AddRange(Roles.Select(role => new Claim(ClaimTypes.Role, role)));
        claims.AddRange(Groups.Select(group => new Claim(ClaimTypes.GroupSid, group)));

        return claims.ToArray();
    }
}