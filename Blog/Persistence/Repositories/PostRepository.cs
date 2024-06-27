using Blog.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Persistence.Repositories
{
    public interface IPostRepository : IBaseRepository<Post>
    {
        Task<IEnumerable<Post>> GetPostsAsync(
          int? CategoryId,  int pageNumber, int pageSize);
    }
    #region Implementation
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(AppDbContext context)
            : base(context)
        {

        }

        public async Task<IEnumerable<Post>> GetPostsAsync(int? CategoryId, int pageNumber, int pageSize)
        {
            // collection to start from
            var collection = context.Posts as IQueryable<Post>;

            if (CategoryId != null)
            {
               
                collection = collection.Where(c => c.CategoryId == CategoryId);
            }          
            var totalItemCount = await collection.CountAsync();          

            var collectionToReturn = await collection.OrderBy(c => c.CreatedDate)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return collectionToReturn;
        }
    }
    #endregion
}


