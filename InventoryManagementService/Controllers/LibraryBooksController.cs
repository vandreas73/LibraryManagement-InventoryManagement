﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryManagementService;
using InventoryManagementService.Models;
using static System.Reflection.Metadata.BlobBuilder;
using InventoryManagementService.DTOs;
using AutoMapper;

namespace InventoryManagementService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LibraryBooksController : ControllerBase
	{
		private readonly LibraryContext _context;
		private readonly IMapper mapper;
		private readonly CatalogClient catalogClient;

		public LibraryBooksController(LibraryContext context, IMapper mapper)
		{
			_context = context;
			this.mapper = mapper;
			catalogClient = new CatalogClient("https://catalog-container-app.happyrock-19bd815d.northeurope.azurecontainerapps.io/", new HttpClient());
		}

		// GET: api/LibraryBooks
		[HttpGet]
		public async Task<ActionResult<IEnumerable<LibraryBookDTO>>> GetLibraryBook()
		{
			var libraryBooks = await _context.LibraryBook.ToListAsync();
			return Ok(mapper.Map<IEnumerable<LibraryBookDTO>>(libraryBooks));
		}

		// GET: api/LibraryBooks/5
		[HttpGet("{id}")]
		public async Task<ActionResult<LibraryBookDTO>> GetLibraryBook(int id)
		{
			var libraryBook = await _context.LibraryBook.FindAsync(id);

			if (libraryBook == null)
			{
				return NotFound();
			}

			return Ok(mapper.Map<LibraryBookDTO>(libraryBook));
		}

		// PUT: api/LibraryBooks/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutLibraryBook(int id, LibraryBookDTO libraryBookDto)
		{
			if (id != libraryBookDto.Id)
			{
				return BadRequest();
			}

			if (!_context.Libraries.Any(l => l.Id == libraryBookDto.LibraryId))
			{
				return BadRequest("Library does not exist");
			}

			try
			{
				var book = await catalogClient.BooksGETAsync(libraryBookDto.BookId);
			}
			catch (ApiException ex) when (ex.StatusCode == 404)
			{
				return BadRequest("Book does not exist");
			}

			var libraryBook = mapper.Map<LibraryBook>(libraryBookDto);

			_context.Entry(libraryBook).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!LibraryBookExists(id))
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

		// POST: api/LibraryBooks
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<LibraryBookDTO>> PostLibraryBook(LibraryBookDTO libraryBookDto)
		{
			if (libraryBookDto.Id != 0)
			{
				return BadRequest("Id must be empty");
			}

			if (!_context.Libraries.Any(l => l.Id == libraryBookDto.LibraryId))
			{
				return BadRequest("Library does not exist");
			}

			try
			{
				var book = await catalogClient.BooksGETAsync(libraryBookDto.BookId);
			}
			catch (ApiException ex) when (ex.StatusCode == 404)
			{
				return BadRequest("Book does not exist");
			}

			var libraryBook = mapper.Map<LibraryBook>(libraryBookDto);

			_context.LibraryBook.Add(libraryBook);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetLibraryBook", new { id = libraryBook.Id }, mapper.Map<LibraryBookDTO>(libraryBook));
		}

		// DELETE: api/LibraryBooks/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteLibraryBook(int id)
		{
			var libraryBook = await _context.LibraryBook.FindAsync(id);
			if (libraryBook == null)
			{
				return NotFound();
			}

			_context.LibraryBook.Remove(libraryBook);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		[HttpGet("{id}/books")]
		public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooksOfLibrary(int id)
		{
			var libraryBooks = await _context.LibraryBook.Where(lb => lb.LibraryId == id).ToListAsync();
			var books = new List<BookDTO>();
			foreach (var libraryBook in libraryBooks)
			{
				books.Add(await catalogClient.BooksGETAsync(libraryBook.BookId));
			}
			return Ok(books);
		}

		[HttpGet("books/{bookId}")]
		public async Task<ActionResult<IEnumerable<LibraryDTO>>> GetLibrariesHavingBook(int bookId)
		{
			var libraries = await _context.Libraries.Include(l => l.LibraryBooks)
				.Where(l => l.LibraryBooks.Any(lb => lb.BookId == bookId)).ToListAsync();
			return Ok(mapper.Map<IEnumerable<LibraryDTO>>(libraries));
		}

		private bool LibraryBookExists(int id)
		{
			return _context.LibraryBook.Any(e => e.Id == id);
		}
	}
}
