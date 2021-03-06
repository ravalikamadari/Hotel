using Microsoft.AspNetCore.Mvc;
using Hotel.Models;
using Hotel.Repositories;
using Hotel.DTOs;

namespace Hotel.Controllers;

[ApiController]
[Route("api/staff")]
public class StaffController : ControllerBase
{
    private readonly ILogger<StaffController> _logger;
    private readonly IStaffRepository _staff;
    private readonly IRoomRepository _room;

    public StaffController(ILogger<StaffController> logger , IStaffRepository staff , IRoomRepository room)
    {
        _logger = logger;
        _staff = staff;
        _room = room;
    }

    [HttpGet]
    public async Task<ActionResult<List<Staff>>> GetList()
    {
        var res = await _staff.GetList();

        return Ok(res.Select(x => x.asDto));

    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Staff>> GetById([FromRoute] int id)
    {
        var res = await _staff.GetById(id);

        if (res == null)
            return NotFound();

        var dto = res.asDto;
        
        dto.Rooms = (await _room.GetListByGuestId(id)).Select(x => x.asDto).ToList();

        return Ok(dto);

    }

    [HttpPost]
    public async Task<ActionResult<StaffDTO>> Create([FromBody] StaffCreateDTO Data)
    {
          var toCreateStaff = new Staff
        {
           
            DateOfBirth = Data.DateOfBirth,
            Gender = Data.Gender,
            Mobile = Data.Mobile,
            Name = Data.Name.Trim(),
            Shift = Data.Shift,
        };

        var res = await _staff.Create(toCreateStaff);

        return StatusCode(StatusCodes.Status201Created, res.asDto);


    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] StaffUpdateDTO Data)
    {
        var existingStaff = await _staff.GetById(id);

        if (existingStaff == null)
            return NotFound();

        var toUpdateStaff = existingStaff with
        {
          
            Mobile = Data.Mobile,
            Shift = Data.Shift,
         
        };

        var didUpdate = await _staff.Update(toUpdateStaff);

        if (!didUpdate)
            return StatusCode(StatusCodes.Status500InternalServerError);

        return NoContent();


    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        var existing = await _staff.GetById(id);
        if (existing is null)
            return NotFound("No staff found with given Id");

        var didDelete = await _staff.Delete(id);

        return NoContent();

    }
}
