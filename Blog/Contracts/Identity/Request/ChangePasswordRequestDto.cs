﻿namespace Blog.Contracts.Identity.Request
{
    public class ChangePasswordRequestDto
    {
        public string Id { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
