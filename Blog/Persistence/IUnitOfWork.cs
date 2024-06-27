using Blog.Persistence.Repositories;

namespace Blog.Persistence
{
    public interface IUnitOfWork
    {
        IPostRepository Post { get; }
        ICategoryRepository Category { get; }
        ICommentRepository Comment { get; }
        Task Commit();

    }
}
