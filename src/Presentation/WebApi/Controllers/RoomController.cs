using ApplicationCore.Command.Features.Rooms.Commands;
using ApplicationCore.Query.Features.Rooms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("rooms")]
public class RoomController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoomController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("best-reservation-option")]
    public async Task<ActionResult<string>> Add(
        [FromQuery] int numberOfGuests)
    {
        var result = await _mediator.Send(new GetBestReservationOptionQuery() { NumberOfGuests = numberOfGuests});
        return result;
    }
}