using Microsoft.AspNetCore.Mvc;
using MiPrimerApi.Models;
using MiPrimerApi.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MiPrimerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private static readonly List<ValueDto> Values = new()
        {
            new ValueDto
            {
                Id = 1,
                Name = "Primer valor"
            },
            new ValueDto
            {
                Id = 2,
                Name = "Segundo valor",
            }
        };


        [HttpGet]
        [ProducesResponseType<IEnumerable<ValueDto>>(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ValueDto>> GetAll()
        {
            return Ok(Values);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType<ValueDto>(StatusCodes.Status200OK)]
        [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
        public ActionResult<string> GetById([FromRoute] int id)
        {
            //busco dentro de mi lista (bd) de valores el que corresponda con el Id = "id"
            var value = Values.FirstOrDefault(x => x.Id == id);

            if (value == null) 
            {
                return NotFound(new ErrorResponse { Code = "0001", Message = "No se encontró el valor esperado" });
            }

            return Ok(value);
            
        }

        // POST api/<ValuesController>
        [HttpPost]
        [ProducesResponseType<ValueDto>(StatusCodes.Status201Created)]
        [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
        public ActionResult<ValueDto> Post([FromBody] PostValueDto dto)
        {
            if(string.IsNullOrEmpty(dto.Name)) 
            { 
                return BadRequest(new ErrorResponse 
                { 
                    Code = "0002", 
                    Message = "El nombre no puede estar vacío" 
                });
            }

            //calcular previamente el Id a insertar del objeto dto
            var newId = Values.Max(v => v.Id) + 1;
            var newValue = new ValueDto
            {
                Id = newId,
                Name = dto.Name
            };

            Values.Add(newValue);
            return CreatedAtAction(nameof(GetById), new { id = newValue.Id }, newValue);
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
        public IActionResult Put([FromRoute] int id, [FromBody] ValueDto dto)
        {
            //Si yo quiero pisar el elemento Id entonces debo consultarlo a mi base de datos
            var value = Values.FirstOrDefault(x => x.Id == id);

            if (value == null) 
            {
                return NotFound(new ErrorResponse { Code = "0001", Message = "No se encontró el valor esperado" });
            }

            value.Name = $"{dto.Name}";
            return NoContent();
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
        public IActionResult Delete([FromRoute] int id)
        {
            var value = Values.FirstOrDefault(x => x.Id == id);
            
            if (value == null)
            {
                return NotFound(new ErrorResponse { Code = "0001", Message = "No se encontró el valor esperado" });
            }

            Values.Remove(value);
            return NoContent();
        }
    }
}
