namespace Blog.Contracts
{
    public static class ApiRoute
    {

        public static class Auth
        {
            public const string Register = "register";
            public const string Login = "login";
            public const string RefreshToken = "refreshToken";
            public const string RevokeToken = "revokeToken";
        }
            public static class Identity
            {
         
                public const string RemoveUserToRoles = "removeUserToRoles";
                public const string AddUserToRoles = "addUserToRoles";
                public const string UpdateAccount = "updateAccount";
                public const string ResetPassword = "resetpassword";
                public const string ConfirmPasswordReset = "confirmpasswordreset";
                public const string VerifyEmail = "verifyEmail";
                public const string ChangePassword = "changePassword";
            }
        public static class Blog
        {
            public const string Create = "createPost";
            public const string Update = "updatePost";
            public const string Delete = "deletePost";
            public const string GetAll = "getAllPosts";
            public const string GetById = "getPostById";


        }
        public static class Category
        {
            public const string Create = "create";
            public const string Update = "update";
            public const string GetAll = "getAllCategories";
            public const string GetById = "getCategoryById";


        }
        public static class Comments
        {
            public const string Create = "create";
            public const string Update = "update";
            public const string GetAll = "getAllComments";
            public const string GetById = "getCommentById";


        }
    }
}
