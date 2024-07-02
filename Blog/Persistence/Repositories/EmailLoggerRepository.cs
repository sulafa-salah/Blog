using Blog.Domain.Models;

namespace Blog.Persistence.Repositories
{
 
     public interface IEmailLoggerRepository : IBaseRepository<EmailLogger>
    {

    }
    #region Implementation
    public class EmailLoggerRepository : BaseRepository<EmailLogger>, IEmailLoggerRepository
    {
        public EmailLoggerRepository(AppDbContext context)
            : base(context)
        {

        }


    }
    #endregion
}
