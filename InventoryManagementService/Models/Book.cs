namespace InventoryManagementService.Models
{
	public class Book
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public int Pages { get; set; }
		public string Language { get; set; }
		public string Publisher { get; set; }
		public string ISBN { get; set; }
		public int PublishingYear { get; set; }
		public int AuthorId { get; set; }
		public string AuthorName { get; set; }
	}
}
