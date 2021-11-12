using FoodInfoAPI.DbContexts;
using FoodInfoAPI.DTOModels;
using FoodInfoAPI.Models;
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

        [HttpGet]
        public ActionResult GetAddedMeals()
        {
            var foodResult = _foodDb.Meals.ToList();
            return Ok(foodResult);
        }

        [HttpPost]
        public ActionResult AddFoodToDB(string secret, [FromBody] AddFood food)
        {
            if (food == null)
                return StatusCode(401, "Send item was unvalid");
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
        public ActionResult AddFoodsToDB(string secret, [FromBody] List<AddFood> food)
        {
            if (food == null)
                return StatusCode(401, "Send item was unvalid");
            var foodPW = Configuration.GetSection("FoodSecret").Value;
            if (secret == foodPW)
            {
                food.ForEach(x => x.ID = 0);
                _foodDb.AddedFood.AddRange(food);
                _foodDb.SaveChanges();
                return Ok(new { successMsg = food });
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { error = "Wrong password" });
            }
        }

        [HttpPost]
        public ActionResult RemoveFoodFromDB(string secret, [FromBody] AddFood food)
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

        [HttpPost]
        public ActionResult AddMealToDB(string secret, [FromBody] MealEaten meal)
        {
            if (meal == null)
                return StatusCode(401, "Send item was unvalid");
            var foodPW = Configuration.GetSection("FoodSecret").Value;
            if (secret == foodPW)
            {
                var entry = _foodDb.Meals.Add(meal);
                _foodDb.SaveChanges();
                return Ok(new { successMsg = entry.Entity });
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { error = "Wrong password" });
            }
        }

        [HttpPost]
        public ActionResult RemoveMealFromDB(string secret, [FromBody] MealEaten meal)
        {
            if (meal == null)
                return StatusCode(401, "Send item was unvalid");
            var foodPW = Configuration.GetSection("FoodSecret").Value;
            if (secret == foodPW)
            {
                _foodDb.Meals.Remove(meal);
                _foodDb.SaveChanges();
                return Ok(new { successMsg = "Successfully removed from DB" });
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { error = "Wrong password" });
            }
        }

        [HttpPost]
        public ActionResult AddFoodToMeal(string secret, [FromBody] MealAddDTO mealAdd)
        {
            if (mealAdd == null || mealAdd.FoodID == 0)
                return StatusCode(401, "Send item was unvalid");
            var foodPW = Configuration.GetSection("FoodSecret").Value;
            if (secret == foodPW)
            {
                var meal = _foodDb.Meals.FirstOrDefault(x => x.ID == mealAdd.MealID);
                if (meal == null)
                    return StatusCode(401, "Could not find specified meal");
                var food = _foodDb.AddedFood.FirstOrDefault(x => x.ID == mealAdd.FoodID);
                if (food != null)
                {
                    meal.EatenFood.Add(food);
                    _foodDb.SaveChanges();
                    return Ok(new { successMsg = meal });
                }
                else
                    return StatusCode(401, "Could not find specified food");
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { error = "Wrong password" });
            }
        }

        [HttpPost]
        public ActionResult RemoveFoodFromMeal(string secret, [FromBody] MealAddDTO mealRemove)
        {
            if (mealRemove == null || mealRemove.FoodID == 0)
                return StatusCode(401, "Send item was unvalid");
            var foodPW = Configuration.GetSection("FoodSecret").Value;
            if (secret == foodPW)
            {
                var meal = _foodDb.Meals.FirstOrDefault(x => x.ID == mealRemove.MealID);
                if (meal == null)
                    return StatusCode(401, "Could not find specified meal");
                var food = _foodDb.AddedFood.FirstOrDefault(x => x.ID == mealRemove.FoodID);
                if (food != null)
                {
                    meal.EatenFood.Remove(food);
                    _foodDb.SaveChanges();
                    return Ok(new { successMsg = meal });
                }
                else
                    return StatusCode(401, "Could not find specified food");
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { error = "Wrong password" });
            }
        }
    }
}
