using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using technical_tests_backend_ssr.Dtos;
using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Services;

namespace technical_tests_backend_ssr.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IMapper _mapper;

        public ProductsController(IProductService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET /api/products
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetAll(CancellationToken ct)
        {
            var products = await _service.GetAllAsync(ct);
            var dto = _mapper.Map<List<ProductReadDto>>(products);
            return Ok(dto);
        }

        // GET /api/products/{id}
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductReadDto>> GetById(int id, CancellationToken ct)
        {
            var product = await _service.GetByIdAsync(id, ct);
            if (product is null)
                return NotFound();

            var dto = _mapper.Map<ProductReadDto>(product);
            return Ok(dto);
        }

        // POST /api/products
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductReadDto>> Create([FromBody] ProductCreateDto model, CancellationToken ct)
        {
            try
            {
                var entity = _mapper.Map<Product>(model);
                var created = await _service.CreateAsync(entity, ct);
                var dto = _mapper.Map<ProductReadDto>(created);
                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUT /api/products/{id}
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductReadDto>> Update(int id, [FromBody] ProductUpdateDto model, CancellationToken ct)
        {
            try
            {
                var entity = _mapper.Map<Product>(model);
                var updated = await _service.UpdateAsync(id, entity, ct);
                var dto = _mapper.Map<ProductReadDto>(updated);
                return Ok(dto);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE /api/products/{id}
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var existing = await _service.GetByIdAsync(id, ct);
            if (existing is null)
                return NotFound();

            await _service.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}
