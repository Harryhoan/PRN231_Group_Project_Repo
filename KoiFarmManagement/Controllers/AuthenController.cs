using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace KoiFarmManagement.Controllers
{
    [EnableCors("Allow")]
    [Route("api/authentication")]
    [ApiController]
    public class AuthenController : BaseController
    {

        public AuthenController()
        {
            
        }
    }
}
