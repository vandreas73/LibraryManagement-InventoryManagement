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
using AutoMapper;
using InventoryManagementService.DTOs;

namespace InventoryManagementService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LibrariesController : ControllerBase
	{
		private readonly LibraryContext _context;
		private readonly IValidator<LibraryDTO> validator;
		private readonly IMapper mapper;

		public LibrariesController(LibraryContext context, IValidator<LibraryDTO> validator, IMapper mapper)
		{
			_context = context;
			this.validator = validator;
			this.mapper = mapper;
		}

		// GET: api/Libraries
		[HttpGet]
		public async Task<ActionResult<IEnumerable<LibraryDTO>>> GetLibraries()
		{
			var libraries = await _context.Libraries.ToListAsync();
			return Ok(mapper.Map<IEnumerable<LibraryDTO>>(libraries));
		}

		// GET: api/Libraries/5
		[HttpGet("{id}")]
		public async Task<ActionResult<LibraryDTO>> GetLibrary(int id)
		{
			var library = await _context.Libraries.FindAsync(id);

			if (library == null)
			{
				return NotFound();
			}

			return Ok(mapper.Map<LibraryDTO>(library));
		}

		// PUT: api/Libraries/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutLibrary(int id, LibraryDTO libraryDto)
		{
			if (id != libraryDto.Id)
			{
				return BadRequest();
			}

			var validationResult = await ValidateUser(libraryDto);
			if (validationResult != null)
			{
				return validationResult;
			}

			var library = mapper.Map<Library>(libraryDto);

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
		public async Task<ActionResult<LibraryDTO>> PostLibrary(LibraryDTO libraryDto)
		{
			var validationResult = await ValidateUser(libraryDto);
			if (validationResult != null)
			{
				return validationResult;
			}

			var library = mapper.Map<Library>(libraryDto);

			_context.Libraries.Add(library);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetLibrary", new { id = library.Id }, mapper.Map<LibraryDTO>(library));
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
		public async Task<ActionResult<IEnumerable<LibraryDTO>>> SearchLibraries([FromQuery] string name = "", string address = "")
		{
			var libraries = await _context.Libraries.Where(l => l.Name.ToLower().Contains(name) && l.Address.ToLower().Contains(address)).ToListAsync();
			if (libraries == null)
			{
				return NotFound();
			}
			return Ok(mapper.Map<IEnumerable<LibraryDTO>>(libraries));
		}

		private bool LibraryExists(int id)
		{
			return _context.Libraries.Any(e => e.Id == id);
		}

		private async Task<ActionResult> ValidateUser(LibraryDTO libraryDto)
		{
			FluentValidation.Results.ValidationResult result = await validator.ValidateAsync(libraryDto);

			if (!result.IsValid)
			{
				var errors = result.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
				return BadRequest(new { Errors = errors });
			}
			return null;
		}
	}
}
