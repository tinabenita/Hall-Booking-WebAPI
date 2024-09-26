using BanquetHallProject.Models;
using Microsoft.EntityFrameworkCore;

namespace BanquetHallProject.Data
{
    public class HallDbContext : DbContext
    {
        public HallDbContext(DbContextOptions<HallDbContext> options) : base(options)
        {

        }
        public DbSet<HallAPIDataModel> HallBookings { get; set; }
    }
}

//Used for interaction with our database
//We use this file to create the DbSet which will be used in our controller. It uses the Entity Framework Core. 