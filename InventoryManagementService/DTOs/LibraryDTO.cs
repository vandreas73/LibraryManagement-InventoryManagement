﻿using InventoryManagementService.Models;

namespace InventoryManagementService.DTOs
{
	public class LibraryDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string ManagerName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string? Website { get; set; }
	}
}
