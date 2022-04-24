﻿using Core.Entities;

namespace Entities.DTOs
{
	public class UserForUpdateDto : IDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
	}
}