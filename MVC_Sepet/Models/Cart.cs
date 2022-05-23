using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Sepet.Models
{
    public class Cart
    {
        Dictionary<int,CartItem> _myCart = new Dictionary<int,CartItem>();
        public List<CartItem> myCart
        {
            get { return _myCart.Values.ToList(); }
        }
        public void AddItem(CartItem cartItem)
        {
            if (_myCart.ContainsKey(cartItem.Id))
            {
                _myCart[cartItem.Id].Quantity += cartItem.Quantity;
                return;
            }
            _myCart.Add(cartItem.Id, cartItem);
        }
    }
}