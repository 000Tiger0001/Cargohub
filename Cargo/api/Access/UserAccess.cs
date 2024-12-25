using Microsoft.EntityFrameworkCore;

public class UserAccess : BaseAccess<User>
{
    public UserAccess(ApplicationDbContext context) : base(context) { }
}