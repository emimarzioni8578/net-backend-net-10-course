using Microsoft.AspNetCore.Mvc;
using MiPrimerApi.Bussiness;
using MiPrimerApi.Entities;
using MiPrimerApi.Exceptions;
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
    public async Task<ActionResult<IEnumerable<User>>> GetUser()
    {
        //devolver el listado de usuarios
        var users = await _service.GetAllUsersAsync();
        return Ok(users);
    }

    // GET: api/User/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<User>> GetUser([FromRoute]int id)
    {
        try
        {
            var user = await _service.GetUserAsync(id);
            return Ok(user);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    // PUT: api/User/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutUser([Required][FromRoute] int? id, [FromBody] User user)
    {
        try
        {
            await _service.UpdateUserAsync(id!.Value, user);
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
    public async Task<ActionResult<User>> PostUser(User user)
    {
        var created = await _service.CreateUserAsync(user);
        
        //devuelve un 201 con el curl para poder hacer get by id
        return CreatedAtAction(nameof(GetUser), new { id = created.Id }, created);
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
