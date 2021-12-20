using OnlineFoodOrdering.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineFoodOrdering.ViewModels
{
    public class ProductModel
    {
        public List<Product> _products { get; set; }

        public List<Product> findAll()
        {
            _products = new List<Product>{new Product()
            {
                Id = "1", Name = "BisiBele Bath", Photo = "BisiBeleBath.jpg", Price = 50.00
            },
            new Product()
            {
                Id = "2",Name="Coffee",Photo="coffe.jpg", Price=15.00
            },
            new Product()
            {
                Id = "3",Name="Dosa",Photo="dosa.jpeg", Price=40.00
            },
              new Product()
            {
                Id = "4",Name="Meals",Photo="meals2.jpg", Price=40.00
            },
            new Product()
            {
                Id = "5",Name="Non veg Meals",Photo="meals3.jpg", Price=60.00
            },
            new Product()
            {
                Id = "6",Name="Poori",Photo="poori.jpg", Price=30.00
            },
              new Product()
            {
                Id = "7",Name="Idli",Photo="Idli.jpg", Price=30.00
            },
              new Product()
            {
                Id = "8",Name="Juice",Photo="juice.jpg", Price=30.00
            },
            new Product()
            {
                Id = "8",Name="Tea",Photo="tea.jpg", Price=10.00
            },
              new Product()
            {
                Id = "10",Name="Breakfast Combo",Photo="breakfast1.jpg", Price=60.00
            }



            };

            return _products;
        }


        public Product find(string id)
        {
            // var returnedObj = SessionHelper.GetObjectFromJson<List<Product>>(HttpContext.Session, "ProductList");

            List<Product> products = findAll();

            var prod = products.Where(a => a.Id == id).FirstOrDefault();
            return prod;


        }
    }
}