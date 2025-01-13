namespace InventoryManagementService.Models
{
	public class LibraryBook
	{
		public int Id { get; set; }
		public int LibraryId { get; set; }
		public Library Library { get; set; }	
		public int BookId { get; set; }
		public int Count { get; set; }
	}
}
