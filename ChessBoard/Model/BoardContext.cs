using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace ChessBoard.Model
{
    class BoardContext : DbContext
    {
        public DbSet<Cell> Cells { get; set; }
        public DbSet<Player> Players { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb; Database=BoardDB; Trusted_Connection=True");
        }      
    }
}
