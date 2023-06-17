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
    public class CategoryController : Controller
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

        // GET: Cat
        public ActionResult Index()
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
            return View(categories);
        }

        

        // GET: Cat/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cat/Create
        [HttpPost]
        public ActionResult Create(CategoryModel category)
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
                            CategoryModel cat = new CategoryModel
                            {
                                CategoryId = (int)reader["CategoryId"],
                                CategoryName = (string)reader["CategoryName"]
                            };
                            categories.Add(cat);
                        }
                    }
                }
            }

            for (int i = 0; i < categories.Count; i++)
            {
                if ( categories[i++].CategoryName == category.CategoryName)
                {
                    TempData["SuccessMessagevalidation"] = "Category Allready Exist";
                    return RedirectToAction("Create");
                }  
            }
            try
            {
                if (ModelState.IsValid)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("INSERT INTO Category (CategoryName) VALUES (@CategoryName)", connection))
                        {
                            command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                            command.ExecuteNonQuery();
                        }
                    }
                    TempData["SuccessMessage"] = "Category created successfully!!";
                    return RedirectToAction("Index");
                }

            }
            catch
            {
                return View();
            }
            return View(category);
        }

        // GET: Cat/Edit/5
        public ActionResult Edit(int id)
        {
            CategoryModel category = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Category WHERE CategoryId = @CategoryId", connection))
                {
                    command.Parameters.AddWithValue("@CategoryId", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            category = new CategoryModel
                            {
                                CategoryId = (int)reader["CategoryId"],
                                CategoryName = (string)reader["CategoryName"]
                            };
                        }
                    }
                }
            }
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Cat/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, CategoryModel category)
        {
            if (id != category.CategoryId)
            {
                return HttpNotFound();
            }
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
                            CategoryModel cat = new CategoryModel
                            {
                                CategoryId = (int)reader["CategoryId"],
                                CategoryName = (string)reader["CategoryName"]
                            };
                            categories.Add(cat);
                        }
                    }
                }
            }

            for (int i = 0; i < categories.Count; i++)
            {
                if (categories[i++].CategoryName == category.CategoryName)
                {
                    TempData["SuccessMessagevalidation"] = "Category Allready Exist please give another which is not exist";
                    return RedirectToAction("Edit");
                }
            }

            try
            {
                if (id != category.CategoryId)
                {
                    return HttpNotFound();
                }
                if (ModelState.IsValid)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand("UPDATE Category SET CategoryName = @CategoryName WHERE CategoryId = @CategoryId", connection))
                        {
                            command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
                            command.Parameters.AddWithValue("@CategoryId", category.CategoryId);
                            command.ExecuteNonQuery();
                        }
                    }
                    TempData["SuccessMessage"] = "Category Edit successfully!!";
                    return RedirectToAction("Index");
                }
                return View(category);
            }
            catch
            {
                return View();
            }
        }

        // GET: Cat/Delete/5
        public ActionResult Delete(int id)
        {
            
            CategoryModel category = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Category WHERE CategoryId = @CategoryId", connection))
                {
                    command.Parameters.AddWithValue("@CategoryId", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            category = new CategoryModel
                            {
                                CategoryId = (int)reader["CategoryId"],
                                CategoryName = (string)reader["CategoryName"]
                            };
                        }
                    }
                }
            }
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Cat/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, CategoryModel category)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("DELETE FROM Category WHERE CategoryId = @CategoryId", connection))
                    {
                        command.Parameters.AddWithValue("@CategoryId", id);
                        command.ExecuteNonQuery();
                    }
                }
                TempData["SuccessMessage"] = "Category delete successfully!!";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // GET: Cat/Details/5
        public ActionResult Details(int id)
        {
            CategoryModel category = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Category WHERE CategoryId = @CategoryId", connection))
                {
                    command.Parameters.AddWithValue("@CategoryId", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            category = new CategoryModel
                            {
                                CategoryId = (int)reader["CategoryId"],
                                CategoryName = (string)reader["CategoryName"]
                            };
                        }
                    }
                }
            }
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
       
    }
}
