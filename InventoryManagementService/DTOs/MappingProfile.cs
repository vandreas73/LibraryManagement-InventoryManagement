using AutoMapper;

namespace InventoryManagementService.DTOs
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Models.Library, LibraryDTO>();
			CreateMap<LibraryDTO, Models.Library>();
			CreateMap<Models.LibraryBook, LibraryBookDTO>();
			CreateMap<LibraryBookDTO, Models.LibraryBook>();
		}
	}
}
