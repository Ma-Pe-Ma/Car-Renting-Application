using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

public class CarRentingDBContext : DbContext
{
    public DbSet<Car> Cars { get; set; }
    public DbSet<Renting> Rentings { get; set; }

    public string DbPath { get; }

    public CarRentingDBContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "car-renting.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}

public class Car
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string CarId { get; set; }

    public string LicensePlateNumber { get; set; }
    public string Manufacturer { get; set; }
    public string Model { get; set; }

    public int Year { get; set; }

    public List<Renting> Rentings { get; } = new();
}

public class Renting
{
    public int RentingId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string CustomerName { get; set; }

    public string CarId { get; set; }
    public Car Car { get; set; }
}