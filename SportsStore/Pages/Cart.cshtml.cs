using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsStore.Infrastructure;
using SportsStore.Models;

namespace SportsStore.Pages
{
    public class CartModel : PageModel
    {
        private IStoreRepository repository;

        public CartModel(IStoreRepository repository, Cart cart)
        {
            this.repository = repository;
            Cart = cart;
        }

        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; } = "/";

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
        }

        public IActionResult OnPost(long productId, string returnUrl)
        {
            Product? product = repository.Products
                .FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                Cart.AddItem(product, 1);
            }

            return RedirectToPage(new { returnUrl = returnUrl });
        }

        // Use the FromForm attribute to explicitly bind the form values
        public IActionResult OnPostRemove([FromForm] long ProductID, [FromForm] string returnUrl)
        {
            // We'll use a simpler approach: create a temporary product with just the ID
            // This avoids potential null references
            Cart.RemoveLine(new Product { ProductID = ProductID });

            return RedirectToPage(new { returnUrl = returnUrl });
        }
    }
}