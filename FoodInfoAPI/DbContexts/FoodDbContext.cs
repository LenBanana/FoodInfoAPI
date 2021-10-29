using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pomelo.EntityFrameworkCore.MySql;
using FoodInfoAPI.Models;
using FoodInfoAPI.DTOModels;

namespace FoodInfoAPI.DbContexts
{
    public class FoodDbContext : DbContext
    {
        public FoodDbContext(DbContextOptions<FoodDbContext> options) : base(options)
        {
            
        }

        public DbSet<FoodCategory> FoodCategories { get; set; }
        public DbSet<Food> Food { get; set; }
        public DbSet<Carbohydrates> Carbohydrates { get; set; }
        public DbSet<AddFoodDTO> AddedFood { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<FoodCategory>(x =>
            {
                x.HasKey(y => y.ID);
                x.HasOne(y => y.Food);
            });
            builder.Entity<Food>(x =>
            {
                x.HasKey(y => y.ID);
                x.HasOne(y => y.Carbohydrates);
            });
            builder.Entity<Carbohydrates>(x =>
            {
                x.HasKey(y => y.ID);
            });
            builder.Entity<AddFoodDTO>(x =>
            {
                x.HasKey(y => y.ID);
            });
            builder.Entity<AddFoodDTO>().Property(x => x.TotalCalories).ValueGeneratedOnAdd();
        }
    }
}
