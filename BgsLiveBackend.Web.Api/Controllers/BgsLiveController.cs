using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace BgsLiveBackend.Web.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class BgsLiveController : ControllerBase
    {
        protected int CurrentUserId =>
    int.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

    }

}
