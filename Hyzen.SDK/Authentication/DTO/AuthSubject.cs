namespace Hyzen.SDK.Authentication.DTO;

public class AuthSubject
{
    public Guid Guid { get; set; } 
    public SubjectType Type { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public List<string> Groups { get; set; } = [];
    public List<string> Roles { get; set; } = [];
    
    public bool HasRole(string roleKey)
    {
        var roleParts = roleKey.Split(':');
        
        if (Roles.Contains(roleKey))
        {
            return true;
        }

        for (int i = 0; i < roleParts.Length; i++)
        {
            var partialRole = string.Join(':', roleParts.Take(i + 1)) + ":*";
            if (Roles.Contains(partialRole))
            {
                return true;
            }
        }

        return false;
    }
        
    public bool HasGroup(string groupKey)
    {
        return Groups.Contains(groupKey);
    }
}