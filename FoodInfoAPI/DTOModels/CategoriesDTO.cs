using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodInfoAPI.DTOModels
{
    public class CategoriesDTO
    {
        public List<string> Categories { get; set; }

        public CategoriesDTO(List<string> categories)
        {
            Categories = categories;
        }
    }
}
