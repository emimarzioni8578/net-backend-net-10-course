using Microsoft.AspNetCore.Mvc;
using MiPrimerApi.Bussiness;
using MiPrimerApi.Entities;
using MiPrimerApi.Exceptions;
using MiPrimerApi.Mappers;
using MiPrimerApi.Models;
using System.ComponentModel.DataAnnotations;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUsersService _service;
    public UsersController(IUsersService service)
    {
        _service = service;
    }

    // GET: api/User
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetUser()
    {
        //devolver el listado de usuarios
        var users = await _service.GetAllUsersAsync();
        return Ok(users.ToDtoList());
    }

    // GET: api/User/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserResponse>> GetUser([FromRoute]int id)
    {
        try
        {
            var user = await _service.GetUserAsync(id);
            var response = user.ToDto();
            return Ok(response);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // PUT: api/User/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutUser([Required][FromRoute] int? id, [FromBody] UserRequest user)
    {
        try
        {
            var entity = user.ToEntity(id!.Value);
            await _service.UpdateUserAsync(id!.Value, entity);
        }
        catch (NotSameIdException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }

        return NoContent();
    }

    // POST: api/User
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<UserResponse>> PostUser(UserRequest request)
    {
        var entity = request.ToEntity();
        var result = await _service.CreateUserAsync(entity);
        var userCreated = result.ToDto();
        //devuelve un 201 con el curl para poder hacer get by id
        return CreatedAtAction(nameof(GetUser), new { id = userCreated.Id }, userCreated);
    }

    // DELETE: api/User/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser([FromRoute] int? id)
    {
        try
        {
            await _service.DeleteUserAsync(id!.Value);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }

        //Devuelvo un 204 sin contenido, porque el recurso ya no existe.
        return NoContent();
    }
}
