﻿namespace ISWBlacklist.Application.DTOs.User
{
    public class UpdateUserResponseDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime DateModified { get; set; }
    }
}
