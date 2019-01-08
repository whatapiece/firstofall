using System;
using FirstOfAll.Application.Interfaces;
using FirstOfAll.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstOfAll.WebApi.Controllers
{
    [Authorize]
    public class CustomerController : ApiController
    {
        private readonly ICustomerAppService _customerAppService;

        public CustomerController(
            ICustomerAppService customerAppService)
        {
            _customerAppService = customerAppService;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("customer-management")]
        public IActionResult Get()
        {
            return Response(_customerAppService.GetAll());
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("customer-management/{id:guid}")]
        public IActionResult Get(Guid id)
        {
            var customerViewModel = _customerAppService.GetId(id);

            return Response(customerViewModel);
        }     

        [HttpPost]
        [Authorize(Policy = "CanWriteCustomerData")]
        [Route("customer-management")]
        public IActionResult Post([FromBody]CustomerViewModel customerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Response(customerViewModel);
            }

            _customerAppService.Register(customerViewModel);

            return Response(customerViewModel);
        }
        
        [HttpPut]
        [Authorize(Policy = "CanWriteCustomerData")]
        [Route("customer-management")]
        public IActionResult Put([FromBody]CustomerViewModel customerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Response(customerViewModel);
            }

            _customerAppService.Update(customerViewModel);

            return Response(customerViewModel);
        }

        [HttpDelete]
        [Authorize(Policy = "CanRemoveCustomerData")]
        [Route("customer-management")]
        public IActionResult Delete(Guid id)
        {
            _customerAppService.Remove(id);
            
            return Response();
        }

    }
}
