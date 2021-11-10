using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerceStarterCode.Data;
using eCommerceStarterCode.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace eCommerceStarterCode.Controllers
{
    [Route("api/shoppingcart")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {

        private ApplicationDbContext _context;

        public ShoppingCartController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("{userId}"), Authorize]
        public IActionResult GetShoppingCart(string userId)
        {
            var products = _context.ShoppingCarts.Include(sc => sc.User).Include(sc => sc.Product).Where(sc => sc.User.Id == userId);
            return Ok(products);
        }

        [HttpGet("total/{userId}"), Authorize]
        public IActionResult GetShoppingCartTotal(string userId)
        {
            var productPrices = _context.ShoppingCarts.Include(sc => sc.User).Include(sc => sc.Product).Where(sc => sc.User.Id == userId).Select(sc => sc.Product.Price);
            double total = 0;
            foreach (double price in productPrices)
            {
                total += price;
            }
            if (total == 0)
            {
                string empty = "Your shopping cart is empty.";
                return Ok(empty);
            }
            else
            {
                string totalToString = "$" + total;
                return Ok(totalToString);
            }
        }

        [HttpDelete("product/{productId}/user/{userId}"), Authorize]
        public IActionResult DeleteProduct(int productId, string userId)
        {
            var product = _context.ShoppingCarts
                .Where(sc => (sc.Product.Id == productId && sc.User.Id == userId))
                .SingleOrDefault();
            _context.Remove(product);
            _context.SaveChanges();
            return StatusCode(204);
        }

        [HttpPost, Authorize]
        public IActionResult PostShoppingCart([FromBody] ShoppingCart value)
        {
            var product = _context.ShoppingCarts
            .Where(sc => sc.Product.Id == value.ProductId && sc.User.Id == value.UserId)
            .SingleOrDefault();
            if (product != null)
            {
                product.Quantity += value.Quantity;
                _context.ShoppingCarts.Update(product);
            }
            else
            {
                _context.ShoppingCarts.Add(value);
            }
            _context.SaveChanges();
            return Ok(value);
        }
    }
}
