using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics
{
    public class CartModel : PageModel
    {
        private readonly IBasketService _basketService;

        public CartModel(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public BasketModel Cart { get; set; } = new BasketModel();        

        public async Task<IActionResult> OnGetAsync()
        {
            string userName = "pepe";

            Cart = await _basketService.GetBasket(userName);            

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveToCartAsync(string productId)
        {
            string userName = "pepe";

            var basket = await _basketService.GetBasket(userName);

            var item = basket.Items.Single(i => i.ProductId == productId);

            basket.Items.Remove(item);

            var updatedBasket = await _basketService.UpdateBasket(basket);

            return RedirectToPage();
        }
    }
}