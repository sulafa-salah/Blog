namespace Blog.Contracts.Identity.Request
{
    public class UserRolesRequestDto
    {
        public Guid UserId { get; set; }
        public List<RoleRequest> Roles { get; set; }
    }
    public class RoleRequest
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }

    }
}
