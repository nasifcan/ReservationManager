﻿using ReservationApi.Model;
using Microsoft.EntityFrameworkCore;

namespace ReservationApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=.\\SQLExpress;DataBase=WMS;Trusted_Connection=true;TrustServerCertificate=true");
        }
        public DbSet<Tables> Tables { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}
