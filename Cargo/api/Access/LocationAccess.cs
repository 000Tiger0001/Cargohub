using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class LocationAccess : BaseAccess<Location>
{
    public LocationAccess(ApplicationDbContext context) : base(context) { }
}