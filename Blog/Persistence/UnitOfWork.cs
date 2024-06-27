using Blog.Persistence.Repositories;

namespace Blog.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext dbContext)
        {
            this._context = dbContext;
          
        }
        private ICategoryRepository _Category;
        private IPostRepository _Post;
        private ICommentRepository _Comment;
        public IPostRepository Post
        {
            get
            {
                if (_Post == null)
                {
                    _Post = new PostRepository(_context);
                }

                return _Post;
            }
        }

        public ICategoryRepository Category
        {
            get
            {
                if (_Category == null)
                {
                    _Category = new CategoryRepository(_context);
                }

                return _Category;
            }
        }
        public ICommentRepository Comment
        {
            get
            {
                if (_Comment == null)
                {
                    _Comment = new CommentRepository(_context);
                }

                return _Comment;
            }
        }

        public async Task Commit()
        {
            await this._context.SaveChangesAsync();
        }
    }
}
