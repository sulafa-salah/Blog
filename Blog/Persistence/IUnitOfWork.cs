using Blog.Persistence.Repositories;

namespace Blog.Persistence
{
    public interface IUnitOfWork
    {
        IPostRepository Post { get; }
        ICategoryRepository Category { get; }
        ICommentRepository Comment { get; }
        IEmailLoggerRepository EmailLogger { get; }
        Task Commit();

    }
}
