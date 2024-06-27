using Blog.Domain.Common;

namespace Blog.Domain.Models
{
    public class Comment : BaseModel
    {
        public Comment()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public Guid  PostId { get; set; }
      
        public string Content { get; set; }
        public Post Post { get; set; }
        public ApplicationUser User { get; set; }

    }
}
