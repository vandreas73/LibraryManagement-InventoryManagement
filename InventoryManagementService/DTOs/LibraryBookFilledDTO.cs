namespace InventoryManagementService.DTOs
{
	public class LibraryBookFilledDTO
	{
		public int Id { get; set; }
		public int LibraryId { get; set; }
		public string LibraryName { get; set; }
		public string LibraryAddress { get; set; }
		public int BookId { get; set; }
		public string BookTitle { get; set; }
		public string BookAuthor { get; set; }
		public int Count { get; set; }
	}
}
