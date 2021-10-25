using FoodInfoAPI.DbContexts;
using FoodInfoAPI.DTOModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public FoodController(FoodDbContext foodDb)
        {
            _foodDb = foodDb;
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
            var foodResult = _foodDb.FoodCategories.Where(x => x.Food.Name.ToLower().Contains(name.ToLower())).ToList();
            return Ok(foodResult.Select(x => x.ToDTO()));
        }

        [HttpGet]
        public ActionResult GetFoodInfoByCalories(double minCalories = double.MinValue, double maxCalories = double.MaxValue)
        {
            var foodResult = _foodDb.FoodCategories.Where(x => x.Food.Calories >= minCalories && x.Food.Calories <= maxCalories).ToList();
            return Ok(foodResult.Select(x => x.ToDTO()));
        }

        [HttpGet]
        public ActionResult GetFoodInfoByNameAndCalories(string name, double minCalories = double.MinValue, double maxCalories = double.MaxValue)
        {
            var foodResult = _foodDb.FoodCategories.Where(x =>
                x.Food.Name.ToLower().Contains(name.ToLower()) &&
                x.Food.Calories >= minCalories &&
                x.Food.Calories <= maxCalories
            ).ToList();
            return Ok(foodResult.Select(x => x.ToDTO()));
        }

        [HttpGet]
        public ActionResult GetFoodInfoByCategory(string category)
        {
            var foodResult = _foodDb.FoodCategories.Where(x =>
                x.Category.ToLower() == category.ToLower()
            ).ToList();
            return Ok(foodResult.Select(x => x.ToDTO()));
        }

        [HttpGet]
        public ActionResult GetFoodCategories()
        {
            var foodResult = _foodDb.FoodCategories.Select(x => x.Category).Distinct().ToList();
            return Ok(new CategoriesDTO(foodResult));
        }
    }
}
