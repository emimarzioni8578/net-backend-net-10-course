using Microsoft.AspNetCore.Mvc;

namespace MiPrimerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValoresController : ControllerBase
    {
        private static readonly List<ValueDto> Values = new()
        {
            new ValueDto
            {
                Id = 1,
                Name = "Primer valor",
                Description = "Este es el primer registro de ejemplo",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new ValueDto
            {
                Id = 2,
                Name = "Segundo valor",
                Description = "Este es el segundo registro de ejemplo",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ValueDto>), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ValueDto>> GetAll()
        {
            return Ok(Values);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ValueDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ValueDto> GetById([FromRoute] int id)
        {
            var value = Values.FirstOrDefault(x => x.Id == id);

            if (value is null)
                return NotFound($"No existe un valor con el id {id}");

            return Ok(value);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ValueDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<ValueDto> Create([FromBody] CreateValueDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("El nombre es obligatorio.");

            var newId = Values.Any()
                ? Values.Max(x => x.Id) + 1
                : 1;

            var value = new ValueDto
            {
                Id = newId,
                Name = request.Name,
                Description = request.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            Values.Add(value);

            return CreatedAtAction(
                nameof(GetById),
                new { id = value.Id },
                value
            );
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update(
            [FromRoute] int id,
            [FromBody] UpdateValueDto request)
        {
            if (id <= 0)
                return BadRequest("El id debe ser mayor a cero.");

            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("El nombre es obligatorio.");

            var value = Values.FirstOrDefault(x => x.Id == id);

            if (value is null)
                return NotFound($"No existe un valor con el id {id}");

            value.Name = request.Name;
            value.Description = request.Description;
            value.IsActive = request.IsActive;
            value.UpdatedAt = DateTime.UtcNow;

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete([FromRoute] int id)
        {
            var value = Values.FirstOrDefault(x => x.Id == id);

            if (value is null)
                return NotFound($"No existe un valor con el id {id}");

            Values.Remove(value);

            return NoContent();
        }
    }

    public class ValueDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateValueDto
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }

    public class UpdateValueDto
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }
}