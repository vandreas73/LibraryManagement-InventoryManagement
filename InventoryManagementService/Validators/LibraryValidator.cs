using FluentValidation;
using InventoryManagementService.Models;

namespace InventoryManagementService.Validators
{
	public class LibraryValidator : AbstractValidator<Library>
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
