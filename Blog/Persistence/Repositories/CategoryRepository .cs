using Blog.Domain.Models;

namespace Blog.Persistence.Repositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {

    }
    #region Implementation
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context)
            : base(context)
        {

        }


    }
    #endregion
}
