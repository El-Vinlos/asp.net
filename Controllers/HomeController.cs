using baitap.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace baitap.Controllers;

public class HomeController : Controller
{
    private readonly string _connectionString =
        "Server=.;initial catalog=FlorenciaDB;persist security info=True;Integrated Security=SSPI;TrustServerCertificate=True";

    public IActionResult Index()
    {
        var shoes = new List<Product>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            conn.Open();
            
            string sql = @"SELECT TOP 10 ProductId, ProductName, Description, Price, ImageUrl 
                           FROM Products 
                           WHERE CategoryId = 3
                           ORDER BY CreatedAt DESC";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    shoes.Add(new Product
                    {
                        ProductId = (int)reader["ProductId"],
                        ProductName = reader["ProductName"].ToString(),
                        Description = reader["Description"].ToString(),
                        Price = (decimal)reader["Price"],
                        ImageUrl = reader["ImageUrl"].ToString()
                    });
                }
            }
        }

        ViewBag.Shoes = shoes;
        return View();
    }
}