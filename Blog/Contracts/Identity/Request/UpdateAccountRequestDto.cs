namespace Blog.Contracts.Identity.Request
{
    public class UpdateAccountRequestDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
     
      
        public string PhoneNumber { get; set; }
    }
}
