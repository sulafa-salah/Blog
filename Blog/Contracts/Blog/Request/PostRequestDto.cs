namespace Blog.Contracts.Blog
{
    public class PostRequestDto
    {
     
        public string Title { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; }
     
        public IFormFile? Image { get; set; }


    }
}
