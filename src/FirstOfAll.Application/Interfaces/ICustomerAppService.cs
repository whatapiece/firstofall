using System;
using System.Collections.Generic;
using FirstOfAll.Application.ViewModels;
using FirstOfAll.Domain.Interfaces;
using FirstOfAll.Domain.Models;

namespace FirstOfAll.Application.Interfaces
{
    public interface ICustomerAppService : IDisposable
    {
        void Register(CustomerViewModel customerViewModel);
        IEnumerable<CustomerViewModel> GetAll();
        CustomerViewModel GetId(Guid id);
        void Update(CustomerViewModel customerViewModel);
        void Remove(Guid id);
    }
}
