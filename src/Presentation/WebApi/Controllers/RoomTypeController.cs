using ApplicationCore.Command.Features.Rooms;
using ApplicationCore.Command.Features.Rooms.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("room-types")]
    public class RoomTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoomTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Add(
            [FromBody] AddRoomTypeCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }
    }
}
