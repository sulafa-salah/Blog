using Blog.Domain.Models;

namespace Blog.Persistence.Repositories
{
    public interface IPostRepository : IBaseRepository<Post>
    {

    }
    #region Implementation
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(AppDbContext context)
            : base(context)
        {

        }


    }
    #endregion
}


