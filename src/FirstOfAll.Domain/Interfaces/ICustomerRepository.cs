using FirstOfAll.Domain.Models;

namespace FirstOfAll.Domain.Interfaces
{
    public interface ICustomerRepository : IRepositoryBase<Customer>
    {
        Customer GetByEmail(string email);
    }
}