using Microsoft.AspNetCore.Mvc;
using Partie_Font_Office.Helpers;


namespace Partie_Font_Office.Controllers
{
    public class DatabaseController : Controller
    {
        public IActionResult TestConnection()
        {
            var dbHelper = new DatabaseHelper();
            bool isConnected = dbHelper.TestConnection();

            if (isConnected)
            {
                return Content("Database connection successful!");
            }
            else
            {
                return Content("Failed to connect to the database.");
            }
        }
    }
}
