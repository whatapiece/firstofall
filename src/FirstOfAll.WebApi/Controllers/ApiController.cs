using Microsoft.AspNetCore.Mvc;

namespace FirstOfAll.WebApi.Controllers
{
    public abstract class ApiController : ControllerBase
    {
        protected new IActionResult Response(object result = null)
        {
            
            return Ok(new
            {
                success = true,
                data = result
            });
        }
    }
}
