﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FITHAUI.ShoppingCart.UI.Models;
using FITHAUI.ShoppingCart.UI.Repository;
using Microsoft.AspNetCore.Http;
using FITHAUI.ShoppingCart.UI.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace FITHAUI.ShoppingCart.UI.Controllers
{
    public class HomeController : BaseController
    {
        ProductRepository productRepository = new ProductRepository();
        HomeRepository homeRepository = new HomeRepository();
        CategoryRepository categoryRepository = new CategoryRepository();
        //Action Controller
        /// <summary>
        /// NTHanh HIển thị sản phẩm
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var count = homeRepository.GetNumberVisitor();
            ViewBag.NumberVisitAmount = 0;
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("countVisitor")))
            {
                count++;
                HttpContext.Session.SetInt32("countVisitor", count);
                homeRepository.UpdateNumberVisitor(count);
                ViewBag.NumberVisitAmount = homeRepository.GetNumberVisitor();
            }
            else
            {
                
                ViewBag.NumberVisitAmount = homeRepository.GetNumberVisitor();
            }
            //ModelState.Clear();
            var cart = SessionHelper.GetObjectFromJson<List<CartLine>>(HttpContext.Session, "cart");
            if (cart == null)
            {
                ViewBag.ProductNew = productRepository.GetProductsNew();
                ViewBag.ProductHost = productRepository.GetProductsHot();
                ViewBag.Category = categoryRepository.GetAllCategories();
            }
            else
            {
                ViewBag.cart = cart;
                ViewBag.quanty = cart.Sum(x => x.Quantity);
                ViewBag.total = cart.Sum(item => item.Product.ProductPrice * item.Quantity * (100 - item.Product.ProductSale) / 100);
                ViewBag.ProductNew = productRepository.GetProductsNew();
                ViewBag.ProductHost = productRepository.GetProductsHot();
                ViewBag.Category = categoryRepository.GetAllCategories();
            }
            return View();
        }
        /// <summary>
        /// Tìm kiếm sản phẩm theo tên sản phẩm
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        /// NTHanh
        public ActionResult SearchProductByProductName(string productName)
        {
            ViewBag.ProductNew = productRepository.GetProductsNew();
            ViewBag.ProductHost = productRepository.GetProductsHot();
            ViewBag.Category = categoryRepository.GetAllCategories();
            ViewBag.ProductSearch = productRepository.SearchProductByProductName(productName);
            var model = productRepository.SearchProductByProductName(productName);
            return View(model);
        }
        public ActionResult MenuPartial()
        {
            var category = categoryRepository.GetAllCategories();
            return PartialView(category);
        }
        /// <summary>
        /// Chi tiết sản phẩm
        /// </summary>
        /// <returns></returns>
        public ActionResult ProductDetails(int productId)
        {
            ViewBag.ProductNew = productRepository.GetProductsNew();
            ViewBag.ProductHost = productRepository.GetProductsHot();
            ViewBag.Category = categoryRepository.GetAllCategories();
            ViewBag.ProductDetails = productRepository.ProductDetails(productId);
            var model = productRepository.ProductDetails(productId);
            return View(model);
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
