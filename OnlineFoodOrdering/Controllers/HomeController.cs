using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OnlineFoodOrdering.Helpers;
using OnlineFoodOrdering.Models;
using OnlineFoodOrdering.ViewModels;
using Stripe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineFoodOrdering.Controllers
{
    public class HomeController : Controller
    {
        public IConfiguration Configuration { get; }
        public HomeController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Product()
        {
            ProductModel productModel = new ProductModel();
            ViewBag.products = productModel.findAll();
           
            return View();
        }
        [HttpGet]
        public IActionResult Feedback()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Feedback(Feedback feedback)
        {
            string connectionString = Configuration["ConnectionStrings:Myconnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))

            {
                string sql = $"Insert Into Feedback (Email, Rating) Values ('{feedback.Email}','{feedback.Rating}')";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Registration registration)
        {
            string connectionString = Configuration["ConnectionStrings:Myconnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))

            {
                string sql = $"Insert Into Registration (UserName, Email, Password) Values ('{registration.UserName}', '{registration.Email}','{registration.Password}')";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return RedirectToAction("Login");
        }
        public IActionResult Cart()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            ViewBag.total = cart.Sum(item => item.Product.Price * item.Quantity);
            ViewBag.total = Math.Round(ViewBag.total, 2);
            return View();
        }
        private int isExist(string id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }
        public IActionResult Buy(string id)
        {
            ProductModel productModel = new ProductModel();
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            {
                List<Item> cart = new List<Item>();
                cart.Add(new Item { Product = productModel.find(id), Quantity = 1 });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new Item { Product = productModel.find(id), Quantity = 1 });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Cart");
        }

        public IActionResult Remove(string id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            int index = isExist(id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Cart");

        }
        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Checkout()
        {
            
                return RedirectToAction("CustomerDetails");
            } 
            [HttpPost]
            public IActionResult Processing(string stripeToken, string stripeEmail)
            {
                var optionsCust = new CustomerCreateOptions
                {
                    Email = stripeEmail,
                    Name = "Robert",
                    Phone = "04-234567"

                };
                var serviceCust = new CustomerService();
                Customer customer = serviceCust.Create(optionsCust);
                var optionsCharge = new ChargeCreateOptions
                {
                    /*Amount = HttpContext.Session.GetLong("Amount")*/
                    Amount = Convert.ToInt64(TempData["TotalAmount"]),
                    Currency = "USD",
                    Description = "Buying Flowers",
                    Source = stripeToken,
                    ReceiptEmail = stripeEmail,

                };
                var service = new ChargeService();
                Charge charge = service.Create(optionsCharge);
                if (charge.Status == "succeeded")
                {
                    string BalanceTransactionId = charge.BalanceTransactionId;
                    ViewBag.AmountPaid = Convert.ToDecimal(charge.Amount) % 100 / 100 + (charge.Amount) / 100;
                    ViewBag.BalanceTxId = BalanceTransactionId;
                    ViewBag.Customer = customer.Name;
                    //return View();
                }

                return View();
            }



            public IActionResult CustomerDetails()
            {
                return View();
            }

            [HttpPost]
            public IActionResult CustomerDetails(CustomerDetails customerDetails)
            {
                string connectionString = Configuration["ConnectionStrings:Myconnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))

                {
                    string sql = $"Insert Into CustomerDetails (FullName, Email, Address, City, State, Zip) Values ('{customerDetails.FullName}', '{customerDetails.Email}','{customerDetails.Address}','{customerDetails.City}','{customerDetails.State}', '{customerDetails.Zip}')";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                return RedirectToAction("Success");
            }

        } } 
