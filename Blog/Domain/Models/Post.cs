using Blog.Domain.Common;

namespace Blog.Domain.Models
{
    public class Post : BaseModel
    {
        public Post()
        {
            Id= Guid.NewGuid();
            Comments=new HashSet<Comment>();
        }
        public Guid Id { get; set; }
        public string Title { get; set; }
       
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageLocalPath { get; set; }
       

        public int CategoryId { get; set; }

        #region Relationship
        public Category Category { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ApplicationUser User { get; set; }

        #endregion

    }
}
