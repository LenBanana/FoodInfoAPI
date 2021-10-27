using FoodInfoAPI.DbContexts;
using FoodInfoAPI.DTOModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodInfoAPI.Controllers
{
    [Route("[controller]/[action]")]
    public class FoodController : Controller
    {
        FoodDbContext _foodDb;
        IConfiguration Configuration;
        public FoodController(FoodDbContext foodDb, IConfiguration config)
        {
            _foodDb = foodDb;
            Configuration = config;
        }

        [HttpGet]
        public ActionResult GetFoodRange(int skip)
        {
            var foodResult = _foodDb.FoodCategories.Include(x => x.Food).Skip(skip).Take(10).ToList();
            return Ok(foodResult.Select(x => x.ToDTO()));
        }

        [HttpGet]
        public ActionResult GetFoodInfoByName(string name)
        {
            var foodResult = _foodDb.FoodCategories.Where(x => 
                x.Food.Name.ToLower().Contains(name.ToLower())
            ).ToList();
            var result = foodResult.Select(x => x.ToDTO()).Take(250);
            return Ok(new { Result = result, SearchCount = foodResult.Count });
        }

        [HttpGet]
        public ActionResult GetFoodInfoByCategory(string category)
        {
            var foodResult = _foodDb.FoodCategories.Where(x =>
                x.Category.ToLower() == category.ToLower()
            ).ToList();
            var result = foodResult.Select(x => x.ToDTO()).Take(250);
            return Ok(new { Result = result, SearchCount = foodResult.Count });
        }

        [HttpGet]
        public ActionResult GetFoodInfoByNameAndCategory(string name, string category)
        {
            var foodResult = _foodDb.FoodCategories.Where(x =>
                x.Category.ToLower() == category.ToLower() &&
                x.Food.Name.ToLower().Contains(name.ToLower())
            ).ToList();
            var result = foodResult.Select(x => x.ToDTO()).Take(250);
            return Ok(new { Result = result, SearchCount = foodResult.Count });
        }

        [HttpGet]
        public ActionResult GetFoodInfoByCalories(double minCalories = double.MinValue, double maxCalories = double.MaxValue)
        {
            var foodResult = _foodDb.FoodCategories.Where(x => x.Food.Calories >= minCalories && x.Food.Calories <= maxCalories).ToList();
            var result = foodResult.Select(x => x.ToDTO()).Take(250);
            return Ok(new { Result = result, SearchCount = foodResult.Count });
        }

        [HttpGet]
        public ActionResult GetFoodInfoByNameAndCalories(string name, double minCalories = double.MinValue, double maxCalories = double.MaxValue)
        {
            var foodResult = _foodDb.FoodCategories.Where(x =>
                x.Food.Name.ToLower().Contains(name.ToLower()) &&
                x.Food.Calories >= minCalories &&
                x.Food.Calories <= maxCalories
            ).ToList();
            var result = foodResult.Select(x => x.ToDTO()).Take(250);
            return Ok(new { Result = result, SearchCount = foodResult.Count });
        }

        [HttpGet]
        public ActionResult GetFoodCategories()
        {
            var foodResult = _foodDb.FoodCategories.Select(x => x.Category).Distinct().ToList();
            return Ok(new CategoriesDTO(foodResult));
        }

        [HttpGet]
        public ActionResult GetAddedFood()
        {
            var foodResult = _foodDb.AddedFood.ToList();
            return Ok(foodResult);
        }

        [HttpPost]
        public ActionResult AddFoodToDB(string secret, [FromBody] AddFoodDTO food)
        {
            var foodPW = Configuration.GetSection("FoodSecret").Value;
            if (secret == foodPW)
            {
                var entry = _foodDb.AddedFood.Add(food);
                _foodDb.SaveChanges();
                return Ok(new { successMsg = entry.Entity });
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { error = "Wrong password" });
            }
        }

        [HttpPost]
        public ActionResult RemoveFoodFromDB(string secret, [FromBody] AddFoodDTO food)
        {
            var foodPW = Configuration.GetSection("FoodSecret").Value;
            if (secret == foodPW)
            {
                _foodDb.AddedFood.Remove(food);
                _foodDb.SaveChanges();
                return Ok(new { successMsg = "Successfully removed from DB" });
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { error = "Wrong password" });
            }
        }
    }
}
