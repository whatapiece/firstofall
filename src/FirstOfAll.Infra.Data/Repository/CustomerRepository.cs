using System.Linq;
using FirstOfAll.Domain.Interfaces;
using FirstOfAll.Domain.Models;
using FirstOfAll.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FirstOfAll.Infra.Data.Repository
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(FirstOfAllContext context)
            : base(context)
        {

        }

        public Customer GetByEmail(string email)
        {
            return DbSet.AsNoTracking().FirstOrDefault(c => c.Email == email);
        }
    }
}
