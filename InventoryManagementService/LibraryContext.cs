using InventoryManagementService.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementService
{
	public class LibraryContext : DbContext
	{
		public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
		{
		}
		public DbSet<Library> Libraries { get; set; }
		public DbSet<InventoryManagementService.Models.LibraryBook> LibraryBook { get; set; } = default!;
	}
}
