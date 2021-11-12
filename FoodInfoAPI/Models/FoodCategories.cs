using FoodInfoAPI.DTOModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodInfoAPI.Models
{
    public class FoodCategory
    {
        [Key]
        public int ID { get; set; }
        public string URL { get; set; }
        public string Category { get; set; }
        public virtual Food Food { get; set; }
        public int FoodID { get; set; }

        public FoodCategoryDTO ToDTO()
        {
            return FoodCategoryDTO.FromFoodCategory(this);
        }

        public FoodCategory()
        {

        }
    }

    public class Food
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public double CalorificValue { get; set; }
        public double Calories { get; set; }
        public double Protein { get; set; }
        public virtual Carbohydrates Carbohydrates { get; set; }
        public int CarbohydratesID { get; set; }
        public double Fat { get; set; }
        public double CarbohydrateExchange { get; set; }
        public double Salt { get; set; }

        public FoodDTO ToDTO()
        {
            return FoodDTO.FromFood(this);
        }

        public Food()
        {

        }
    }

    public class Carbohydrates
    {
        [Key]
        public int ID { get; set; }
        public double Carbohydrate { get; set; }
        public double SugarPortion { get; set; }

        public CarbohydratesDTO ToDTO()
        {
            return CarbohydratesDTO.FromCarbohydrates(this);
        }
        public Carbohydrates()
        {

        }
    }

    public class AddFood
    {
        [Key]
        public int ID { get; set; }
        public string Category { get; set; }
        public double Calories { get; set; }
        public string Name { get; set; }
        public DateTime DateAdded { get; set; }
        public double Weight { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public double TotalCalories { get; private set; }

        public AddFood()
        {

        }
    }

    public class MealEaten
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime DateAdded { get; set; }
        public virtual List<AddFood> EatenFood { get; set; }
        public virtual double TotalMealCalories => EatenFood.Sum(x => x.TotalCalories);
        public virtual double TotalMealWeight => EatenFood.Sum(x => x.Weight);
    }
}
