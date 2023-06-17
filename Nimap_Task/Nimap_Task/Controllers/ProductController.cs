using Nimap_Task.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nimap_Task.Controllers
{
    public class ProductController : Controller
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        public int totalPages;

        // GET: Product
        public ActionResult Index(int page = 1)
        {
            const int pageSize = 10;
            List<ProductModel> products = new List<ProductModel>();
            int totalCount = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Retrieve the total count of products
                using (SqlCommand countCommand = new SqlCommand("SELECT COUNT(*) FROM Product", connection))
                {
                    totalCount = (int)countCommand.ExecuteScalar();
                }

                // Calculate the number of pages
                totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                // Adjust the page number if it exceeds the total pages
                if (page > totalPages)
                {
                    page = totalPages;
                }

                // Calculate the skip count
                int skipCount = (page - 1) * pageSize;

                // Retrieve the products for the specified page

                // assending records-
                //using (SqlCommand command = new SqlCommand("SELECT TOP (@PageSize) p.ProductId, p.ProductName, p.CategoryId, c.CategoryName FROM Product p INNER JOIN Category c ON p.CategoryId = c.CategoryId WHERE  p.ProductId NOT IN (SELECT TOP (@SkipCount) ProductId FROM Product ORDER BY ProductId ASC) ORDER BY p.ProductId ASC", connection))
                //{

                //Decending Records-
                using (SqlCommand command = new SqlCommand("SELECT TOP (@PageSize) p.ProductId, p.ProductName, p.CategoryId, c.CategoryName FROM Product p INNER JOIN Category c ON p.CategoryId = c.CategoryId WHERE p.ProductId NOT IN (SELECT TOP (@SkipCount) ProductId FROM Product ORDER BY ProductId DESC) ORDER BY p.ProductId DESC", connection))
                {
                    command.Parameters.AddWithValue("@PageSize", pageSize);
                    command.Parameters.AddWithValue("@SkipCount", skipCount);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductModel product = new ProductModel
                            {
                                ProductId = (int)reader["ProductId"],
                                ProductName = (string)reader["ProductName"],
                                CategoryId = (int)reader["CategoryId"],
                                CategoryName = (string)reader["CategoryName"]
                            };
                            products.Add(product);
                        }
                    }
                }
            }

            // Pass the product list, current page, and total pages to the view
            ViewBag.Products = products;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(products);
        }



        // GET: Product/Create
        public ActionResult Create()
        {

            List<CategoryModel> categories = new List<CategoryModel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Category", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CategoryModel category = new CategoryModel
                            {
                                CategoryId = (int)reader["CategoryId"],
                                CategoryName = (string)reader["CategoryName"]
                            };
                            categories.Add(category);
                        }
                    }
                }
            }
            ViewBag.Categories = categories;
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(ProductModel product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("INSERT INTO Product ( ProductName, CategoryId) VALUES (@ProductName, @CategoryId)", connection))
                        {
                            command.Parameters.AddWithValue("@ProductName", product.ProductName);
                            command.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                            command.ExecuteNonQuery();
                        }
                    }
                    TempData["SuccessMessage"] = "Product created successfully!!";
                    return RedirectToAction("Index");
                }
                return View(product);
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id )
        {
            List<CategoryModel> categories = new List<CategoryModel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Category", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CategoryModel category = new CategoryModel
                            {
                                CategoryId = (int)reader["CategoryId"],
                                CategoryName = (string)reader["CategoryName"]
                            };
                            categories.Add(category);
                        }
                    }
                }
            }
            ViewBag.Categories = categories;
            
            ProductModel product = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT p.ProductId, p.ProductName, p.CategoryId, c.CategoryName FROM Product p INNER JOIN Category c ON p.CategoryId = c.CategoryId WHERE p.ProductId = @ProductId", connection))
                {
                    command.Parameters.AddWithValue("@ProductId", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            product = new ProductModel
                            {
                                ProductId = (int)reader["ProductId"],
                                ProductName = (string)reader["ProductName"],
                                CategoryId = (int)reader["CategoryId"],
                                CategoryName = (string)reader["CategoryName"]
                            };
                        }
                    }
                }
            }
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ProductModel product)
        {
            try
            {
                if (id != product.ProductId)
                {
                    return HttpNotFound();
                }
                if (ModelState.IsValid)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("UPDATE Product SET ProductName = @ProductName, CategoryId = @CategoryId WHERE ProductId = @ProductId", connection))
                        {
                            command.Parameters.AddWithValue("@ProductName", product.ProductName);
                            command.Parameters.AddWithValue("@CategoryId", product.CategoryId);
                            command.Parameters.AddWithValue("@ProductId", product.ProductId);
                            command.ExecuteNonQuery();
                        }
                    }
                    TempData["SuccessMessage"] = "Product Edit successfully!!";
                    return RedirectToAction("Index");
                }
                return View(product);
            }
            catch (Exception)
            {
                return View();
            }
         
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            ProductModel product = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT p.ProductId, p.ProductName, p.CategoryId, c.CategoryName FROM Product p INNER JOIN Category c ON p.CategoryId = c.CategoryId WHERE p.ProductId = @ProductId", connection))
                {
                    command.Parameters.AddWithValue("@ProductId", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            product = new ProductModel
                            {
                                ProductId = (int)reader["ProductId"],
                                ProductName = (string)reader["ProductName"],
                                CategoryId = (int)reader["CategoryId"],
                                CategoryName = (string)reader["CategoryName"]
                            };
                        }
                    }
                }
            }
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, ProductModel product)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("DELETE FROM Product WHERE ProductId = @ProductId", connection))
                    {
                        command.Parameters.AddWithValue("@ProductId", id);
                        command.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            ProductModel product = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT p.ProductId, p.ProductName, p.CategoryId, c.CategoryName FROM Product p INNER JOIN Category c ON p.CategoryId = c.CategoryId WHERE p.ProductId = @ProductId", connection))
                {
                    command.Parameters.AddWithValue("@ProductId", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            product = new ProductModel
                            {
                                ProductId = (int)reader["ProductId"],
                                ProductName = (string)reader["ProductName"],
                                CategoryId = (int)reader["CategoryId"],
                                CategoryName = (string)reader["CategoryName"]
                            };
                        }
                    }
                }
            }
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
    }
}
