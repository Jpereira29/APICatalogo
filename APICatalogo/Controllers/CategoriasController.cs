using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        public CategoriasController(IUnitOfWork context, IMapper mapper)
        {
            _uof = context;
            _mapper = mapper;
        }

        [HttpGet("produtos")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasProdutos()
        {
            var categoria = await _uof.CategoriaRepository.GetCategoriasProdutos();
            var categoriaDto = _mapper.Map<List<CategoriaDTO>>(categoria);
            return categoriaDto;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] CategoriasParameters categoriasParameters)
        {
            try
            {
                var categorias = await _uof.CategoriaRepository.GetCategorias(categoriasParameters);

                var metadata = new
                {
                    categorias.TotalCount,
                    categorias.PageSize,
                    categorias.CurrentPage,
                    categorias.TotalPages,
                    categorias.HasNext,
                    categorias.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                var categoriaDto = _mapper.Map<List<CategoriaDTO>>(categorias);
                return categoriaDto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar a sua solicitação.");
            }
        }

        [HttpGet("{id}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            try
            {
                var categoria = await _uof.CategoriaRepository.GetById(p => p.CategoriaId == id);

                if (categoria is null)
                {
                    return NotFound("Categoria não encontrada");
                }

                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

                return Ok(categoriaDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar a sua solicitação.");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(CategoriaDTO categoriaDto)
        {
            try
            {
                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _uof.CategoriaRepository.Add(categoria);
                await _uof.Commit();

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

                return new CreatedAtRouteResult("ObterCategoria",
                    new { id = categoria.CategoriaId }, categoriaDTO);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar a sua solicitação.");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, CategoriaDTO categoriaDto)
        {
            try
            {
                if (id != categoriaDto.CategoriaId)
                {
                    return BadRequest();
                }
                var categoria = _mapper.Map<Categoria>(categoriaDto);
                _uof.CategoriaRepository.Update(categoria);
                await _uof.Commit();

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                 "Ocorreu um problema ao tratar a sua solicitação.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            try
            {
                var categoria = await _uof.CategoriaRepository.GetById(p => p.CategoriaId == id);

                if (categoria is null)
                {
                    return NotFound();
                }
                _uof.CategoriaRepository.Delete(categoria);
                await _uof.Commit();

                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);
                return Ok(categoria);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um problema ao tratar a sua solicitação.");
            }
        }
    }
}
