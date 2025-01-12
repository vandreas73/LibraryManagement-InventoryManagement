namespace InventoryManagementService.Models
{
	public class SeedData
	{
		public static void Initialize(IServiceProvider serviceProvider)
		{
			var context = serviceProvider.GetRequiredService<LibraryContext>();
			context.Database.EnsureCreated();
			// Look for any libraries.
			if (context.Libraries.Any())
			{
				return;   // DB has been seeded
			}
			context.Libraries.AddRange(
				new Library
				{
					Name = "Illyés Gyula Megyei Könyvtár",
					Address = "7100 Szekszárd, Hunyadi u. 9.",
					ManagerName = "Liebhauser János",
					Email = "tolnalib@igyuk.hu",
					Phone = "20/ 403-73 72",
					Website = "https://www.igyuk.hu/"
				},
				new Library
				{
					Name = "Könyvtár és Közművelődési Intézmény",
					Address = "7100 Szekszárd, Széchenyi tér 1.",
					ManagerName = "Kovácsné Szabó Katalin",
					Email = "a@a.com",
					Phone = "74/ 312- 170"
				}
				);
			context.SaveChanges();
		}
	}
}
