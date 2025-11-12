using Microsoft.AspNetCore.Identity;

namespace MyMvcApp.Data.Entities.Identity;

public class RoleEntity : IdentityRole<int>
{
    public RoleEntity(){}
    
    public RoleEntity(string name){this.Name =  name;}
    
    public ICollection<UserRoleEntity> UserRoles { get; set; } = null!;
}