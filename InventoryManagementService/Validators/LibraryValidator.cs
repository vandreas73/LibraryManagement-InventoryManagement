using FluentValidation;
using InventoryManagementService.DTOs;
using InventoryManagementService.Models;

namespace InventoryManagementService.Validators
{
	public class LibraryValidator : AbstractValidator<LibraryDTO>
	{
		public LibraryValidator()
		{
			RuleFor(x => x.Name).NotEmpty();
			RuleFor(x => x.Address).NotEmpty();
			RuleFor(x => x.ManagerName).NotEmpty();
			RuleFor(x => x.Email).NotEmpty().EmailAddress();
			RuleFor(x => x.Phone).NotEmpty();
		}
	}
}
