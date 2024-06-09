using Contracts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        //private readonly IRepositoryManager _serviceManager;
        //private readonly IMediator _mediator;
        public ApiController()
        {

        }
        public ApiController(IRepositoryManager serviceManager)
        {
            //_serviceManager = serviceManager;
        }

        

        public ApiController(IMediator mediator)
        {
            //_mediator = mediator;
        }
        public ApiController(IServiceManager serviceManager, IMediator mediator)
        {
           // _mediator = mediator;
            //_serviceManager = serviceManager;
        }

        public ApiController(IServiceManager serviceManager)
        {
            //_mediator = mediator;
        }

    }
}
