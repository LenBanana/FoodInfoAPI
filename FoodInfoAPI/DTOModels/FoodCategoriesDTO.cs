using FoodInfoAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodInfoAPI.DTOModels
{
    public class FoodCategoryDTO
    {
        public string URL { get; set; }
        public string Category { get; set; }
        public virtual FoodDTO Food { get; set; }

        public static FoodCategoryDTO FromFoodCategory(FoodCategory fC)
        {
            return new FoodCategoryDTO()
            {
                Category = fC.Category,
                Food = FoodDTO.FromFood(fC.Food),
                URL = fC.URL
            };
        }

        public FoodCategoryDTO()
        {

        }
    }

    public class FoodDTO
    {
        public string Name { get; set; }
        public double CalorificValue { get; set; }
        public double Calories { get; set; }
        public double Protein { get; set; }
        public virtual CarbohydratesDTO Carbohydrates { get; set; }
        public double Fat { get; set; }
        public double CarbohydrateExchange { get; set; }
        public double Salt { get; set; }

        public FoodDTO()
        {

        }

        public static FoodDTO FromFood(Food f)
        {
            return new FoodDTO()
            {
                Name = f.Name,
                Calories = f.Calories,
                CalorificValue = f.CalorificValue,
                CarbohydrateExchange = f.CarbohydrateExchange,
                Carbohydrates = CarbohydratesDTO.FromCarbohydrates(f.Carbohydrates),
                Fat = f.Fat,
                Protein = f.Protein,
                Salt = f.Salt
            };
        }
    }

    public class CarbohydratesDTO
    {
        public static CarbohydratesDTO FromCarbohydrates(Carbohydrates c)
        {
            return new CarbohydratesDTO()
            {
                Carbohydrate = c.Carbohydrate,
                SugarPortion = c.SugarPortion
            };
        }

        public double Carbohydrate { get; set; }
        public double SugarPortion { get; set; }
        public CarbohydratesDTO()
        {

        }
    }
}
