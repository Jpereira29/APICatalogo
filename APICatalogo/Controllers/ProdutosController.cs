using APICatalogo.Models;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        public ProdutosController(IUnitOfWork context)
        {
            _uof = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get()
        {
            try
            {
                var produtos = _uof.ProdutoRepository.Get().ToList();
                if (produtos is null)
                {
                    return NotFound("Produtos não encontrados...");
                }
                return produtos;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                 "Ocorreu um problema ao tratar a sua solicitação.");
            }
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<Produto> Get(int id)
        {
            try
            {
                var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
                if (produto is null)
                {
                    return NotFound("Produto não encontrado...");
                }
                return produto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                 "Ocorreu um problema ao tratar a sua solicitação.");
            }
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            try
            {
                if (produto is null)
                {
                    return BadRequest();
                }
                _uof.ProdutoRepository.Add(produto);
                _uof.Commit();

                return new CreatedAtRouteResult("ObterProduto", new { id = produto.ProdutoId }, produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                 "Ocorreu um problema ao tratar a sua solicitação.");
            }
        }

        [HttpPut("id")]
        public ActionResult Put(int id, Produto produto)
        {
            try
            {
                if (id != produto.ProdutoId)
                {
                    return BadRequest();
                }

                _uof.ProdutoRepository.Update(produto);
                _uof.Commit();

                return Ok(produto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                 "Ocorreu um problema ao tratar a sua solicitação.");
            }
        }

        [HttpDelete("id")]
        public ActionResult Delete(int id)
        {
            try
            {
                var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
                if (produto is null)
                {
                    return NotFound("Produto não encontrado.");
                }
                _uof.ProdutoRepository.Delete(produto);
                _uof.Commit();

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                 "Ocorreu um problema ao tratar a sua solicitação.");
            }
        }
    }
}
