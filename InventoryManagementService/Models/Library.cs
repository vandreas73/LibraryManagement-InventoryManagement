using System.ComponentModel.DataAnnotations;

namespace InventoryManagementService.Models
{
	public class Library
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Address { get; set; }
		[Required]
		public string ManagerName { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public string Phone { get; set; }
		public string Website { get; set; }
	}
}
