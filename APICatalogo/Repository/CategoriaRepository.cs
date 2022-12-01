using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        {
        }

        IEnumerable<Categoria> ICategoriaRepository.GetCategoriasProdutos()
        {
            return Get().Include(x => x.Produtos);
        }
    }
}
