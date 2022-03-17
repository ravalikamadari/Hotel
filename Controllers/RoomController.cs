using Microsoft.AspNetCore.Mvc;
using Hotel.Models;
using Hotel.Repositories;
using Hotel.DTOs;

namespace Hotel.Controllers;

[ApiController]
[Route("api/room")]
public class RoomController : ControllerBase
{
    private readonly ILogger<RoomController> _logger;
    private readonly IRoomRepository _room;
    private readonly IScheduleRepository _schedule;

    public RoomController(ILogger<RoomController> logger, IRoomRepository _room,IScheduleRepository schedule)
    {
        _logger = logger;
        this._room = _room;
        _schedule = schedule;
    }

    [HttpGet]
    public async Task<ActionResult<List<RoomDTO>>> GetList()
    {
          var res = await _room.GetList();

        return Ok(res.Select(x => x.asDto));


    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoomDTO>> GetById([FromRoute] int id)
    {
        var res = await _room.GetById(id);

        if (res is null)
            return NotFound();
        var dto = res.asDto;
        dto.Schedule = (await _schedule.GetListByRoomId(id)).Select(x => x.asDto).ToList();    

        return Ok(dto);
    }

    // [HttpPost]
    // public async Task<ActionResult> Create([FromRoute] int id)
    // {

    // }

    // [HttpPut("{id}")]
    // public async Task<ActionResult> Update([FromRoute] int id)
    // {

    // }

    // [HttpDelete("{id}")]
    // public async Task<ActionResult> Delete([FromRoute] int id)
    // {

    // }
}
