﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrsServer.Data;
using PrsServer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrsServer.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase {
		private readonly PrsDbContext _context;

		public ProductsController(PrsDbContext context) {
			_context = context;
		}

		// GET: api/Products
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Product>>> GetProduct() {
			return await _context.Product.Include(v => v.Vendor).ToListAsync();
		}


		// GET: api/Products/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Product>> GetProduct(int id) {
			var product = await _context.Product.Include(v => v.Vendor).SingleOrDefaultAsync(p=>p.Id==id);


			//var product = await _context.Product.FindAsync(id);

			if (product == null) {
				return NotFound();
			}

			return product;
		}

		// PUT: api/Products/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[HttpPut("{id}")]
		public async Task<IActionResult> PutProduct(int id, Product product) {
			if (id != product.Id) {
				return ValidationProblem();
			}

			_context.Entry(product).State = EntityState.Modified;

			try {
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException) {
				if (!ProductExists(id)) {
					return NotFound();
				}
				else {
					throw;
				}
			}

			return NoContent();
		}

		// POST: api/Products
		// To protect from overposting attacks, enable the specific properties you want to bind to, for
		// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
		[HttpPost]
		public async Task<ActionResult<Product>> PostProduct(Product product) {
			_context.Product.Add(product);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetProduct", new { id = product.Id }, product);
		}

		// DELETE: api/Products/5
		[HttpDelete("{id}")]
		public async Task<ActionResult<Product>> DeleteProduct(int id) {
			var product = await _context.Product.FindAsync(id);
			if (product == null) {
				return NotFound();
			}

			_context.Product.Remove(product);
			await _context.SaveChangesAsync();

			return product;
		}

		private bool ProductExists(int id) {
			return _context.Product.Any(e => e.Id == id);
		}
	}
}
