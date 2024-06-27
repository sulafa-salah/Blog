using Blog.Domain.Common;

namespace Blog.Domain.Models
{
    public class Category : BaseModel
    {
        public Category()
        {
            Posts=new HashSet<Post>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
     
        public ICollection<Post> Posts { get; set; }
    }
}
