using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FoodInfoAPI.DTOModels
{
    public class AddFoodDTO
    {
        [Key]
        public int ID { get; set; }
        public string Category { get; set; }
        public double Calories { get; set; }
        public string Name { get; set; }
        public DateTime DateAdded { get; set; }

        public AddFoodDTO()
        {
            //DateAdded = DateTime.Now;
        }
    }
}
