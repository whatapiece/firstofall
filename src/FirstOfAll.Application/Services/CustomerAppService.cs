using System;
using System.Collections.Generic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FirstOfAll.Application.Interfaces;
using FirstOfAll.Application.ViewModels;
using FirstOfAll.Domain.Interfaces;
using FirstOfAll.Domain.Models;

namespace FirstOfAll.Application.Services
{
    public class CustomerAppService : ICustomerAppService
    {
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;

        public CustomerAppService(IMapper mapper,
                                  ICustomerRepository customerRepository)
        {
            _mapper = mapper;
            _customerRepository = customerRepository;
        }

        public IEnumerable<CustomerViewModel> GetAll()
        {
            return _mapper.Map<IEnumerable<CustomerViewModel>>(_customerRepository.GetAll());
        }

        public CustomerViewModel GetId(Guid id)
        {
            return _mapper.Map<CustomerViewModel>(_customerRepository.GetId(id));
        }

        public void Register(CustomerViewModel customerViewModel)
        {
            _customerRepository.Add(_mapper.Map<Customer>(customerViewModel));
        }

        public void Update(CustomerViewModel customerViewModel)
        {
            _customerRepository.Update(_mapper.Map<Customer>(customerViewModel));
        }

        public void Remove(Guid id)
        {
            _customerRepository.Remove(id);
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
