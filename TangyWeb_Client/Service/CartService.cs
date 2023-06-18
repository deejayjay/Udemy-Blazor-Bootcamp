using Blazored.LocalStorage;
using Tangy_Common;
using TangyWeb_Client.Service.IService;
using TangyWeb_Client.ViewModels;

namespace TangyWeb_Client.Service
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;
        public CartService(ILocalStorageService localStorageService)
        {
            _localStorage = localStorageService;
        }

        public event Action? OnChange;

        public async Task DecrementCart(ShoppingCart cartToDecrement)
        {
            // Get all existing carts from local storage.
            var cart = await _localStorage.GetItemAsync<List<ShoppingCart>>(SD.ShoppingCart);
            
            for (int i = 0; i < cart.Count; i++) 
            {
                if (cart[i].ProductId == cartToDecrement.ProductId 
                    && cart[i].ProductPriceId == cartToDecrement.ProductPriceId)
                {
                    if (cart[i].Count == 1 || cartToDecrement.Count == 0)
                    {
                        cart.Remove(cart[i]);
                    }
                    else
                    {
                        cart[i].Count -= cartToDecrement.Count;
                    }
                }
            }

            await _localStorage.SetItemAsync<List<ShoppingCart>>(SD.ShoppingCart, cart);
            OnChange?.Invoke();
        }

        public async Task IncrementCart(ShoppingCart cartToAdd)
        {
            // Get all existing carts from local storage.
            var cart = await _localStorage.GetItemAsync<List<ShoppingCart>>(SD.ShoppingCart);
            bool itemInCart = false;

            cart ??= new List<ShoppingCart>();

            // If item already exists in the cart, increment count for that record.
            foreach (var item in cart) 
            {
                if (item.ProductId == cartToAdd.ProductId && item.ProductPriceId == cartToAdd.ProductPriceId)
                {
                    itemInCart = true;
                    item.Count += cartToAdd.Count;
                }
            }

            // If item does not exist in the cart, add a new record to the cart.
            if (!itemInCart) 
            {
                cart.Add(new ShoppingCart 
                { 
                    ProductId = cartToAdd.ProductId,
                    ProductPriceId = cartToAdd.ProductPriceId,
                    Count = cartToAdd.Count
                });
            }

            await _localStorage.SetItemAsync<List<ShoppingCart>>(SD.ShoppingCart, cart);
            OnChange?.Invoke();
        }
    }
}
