using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryManagementService;
using InventoryManagementService.Models;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace InventoryManagementService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LibrariesController : ControllerBase
	{
		private readonly LibraryContext _context;
		private readonly IValidator<Library> validator;

		public LibrariesController(LibraryContext context, IValidator<Library> validator)
		{
			_context = context;
			this.validator = validator;
		}

		// GET: api/Libraries
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Library>>> GetLibraries()
		{
			return await _context.Libraries.ToListAsync();
		}

		// GET: api/Libraries/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Library>> GetLibrary(int id)
		{
			var library = await _context.Libraries.FindAsync(id);

			if (library == null)
			{
				return NotFound();
			}

			return library;
		}

		// PUT: api/Libraries/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutLibrary(int id, Library library)
		{
			if (id != library.Id)
			{
				return BadRequest();
			}

			var validationResult = await ValidateUser(library);
			if (validationResult != null)
			{
				return validationResult;
			}

			_context.Entry(library).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!LibraryExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// POST: api/Libraries
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Library>> PostLibrary(Library library)
		{
			var validationResult = await ValidateUser(library);
			if (validationResult != null)
			{
				return validationResult;
			}

			_context.Libraries.Add(library);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetLibrary", new { id = library.Id }, library);
		}

		// DELETE: api/Libraries/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteLibrary(int id)
		{
			var library = await _context.Libraries.FindAsync(id);
			if (library == null)
			{
				return NotFound();
			}

			_context.Libraries.Remove(library);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		[HttpGet("search")]
		public async Task<ActionResult<IEnumerable<Library>>> SearchLibraries([FromQuery] string name)
		{
			var libraries = await _context.Libraries.Where(l => l.Name.Contains(name)).ToListAsync();
			if (libraries == null)
			{
				return NotFound();
			}
			return libraries;
		}

		private bool LibraryExists(int id)
		{
			return _context.Libraries.Any(e => e.Id == id);
		}

		private async Task<ActionResult> ValidateUser(Library request)
		{
			FluentValidation.Results.ValidationResult result = await validator.ValidateAsync(request);

			if (!result.IsValid)
			{
				var errors = result.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
				return BadRequest(new { Errors = errors });
			}
			return null;
		}
	}
}
