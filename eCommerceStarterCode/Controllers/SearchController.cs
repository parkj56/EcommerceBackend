using eCommerceStarterCode.Data;
using eCommerceStarterCode.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eCommerceStarterCode.Controllers
{
    [Route("api/search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private ApplicationDbContext _context;
        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("name/{searchTerm}")]
        public IActionResult SearchByName(string searchTerm)
        {
            var products = _context.Products.Where(p => p.Name.Contains(searchTerm)).ToList();
            return Ok(products);
        }
        [HttpGet("category/{category}")]
        public IActionResult SearchByCategory(string category)
        {
            var products = _context.Products.Where(p => p.Category == (category));
            return Ok(products);
        }
    }
}
