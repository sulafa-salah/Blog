using Blog.Domain.Models;

namespace Blog.Persistence.Repositories
{
    public interface ICommentRepository : IBaseRepository<Comment>
    {

    }
    #region Implementation
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(AppDbContext context)
            : base(context)
        {

        }


    }
    #endregion
}

