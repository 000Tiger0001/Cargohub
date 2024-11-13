using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ClientAccess : BaseAccess<Client>
{
    public ClientAccess(ApplicationDbContext context) : base(context) { }
}