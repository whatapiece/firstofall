using System;
using FirstOfAll.Application.Interfaces;
using FirstOfAll.Application.ViewModels;
using FirstOfAll.WebApi.Controllers.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstOfAll.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CustomerController : ApiController
    {
        private readonly ICustomerAppService _customerAppService;

        public CustomerController(
            ICustomerAppService customerAppService)
        {
            _customerAppService = customerAppService;
        }

        [HttpGet]
        [Route("customer-management")]
        [IsAuthorize]
        [ActionType(AccessType.Read)]
        public IActionResult Get()
        {
            return Response(_customerAppService.GetAll());
        }

        [HttpGet]
        [Route("customer-management/{id:guid}")]
        [IsAuthorize]
        [ActionType(AccessType.Read)]
        public IActionResult Get(Guid id)
        {
            var customerViewModel = _customerAppService.GetId(id);

            return Response(customerViewModel);
        }     

        [HttpPost]
        [Route("customer-management")]
        [IsAuthorize]
        [ActionType(AccessType.Create)]
        public IActionResult Post([FromBody]CustomerViewModel customerViewModel)
        {
            customerViewModel.Id = Guid.NewGuid();

            if (!ModelState.IsValid)
            {
                return Response(customerViewModel);
            }

            _customerAppService.Register(customerViewModel);

            return Response(customerViewModel);
        }
        
        [HttpPut]
        [Route("customer-management")]
        [IsAuthorize]
        [ActionType(AccessType.Update)]
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
        [Route("customer-management")]
        [IsAuthorize]
        [ActionType(AccessType.Delete)]
        public IActionResult Delete(Guid id)
        {
            _customerAppService.Remove(id);
            
            return Response();
        }

    }
}
