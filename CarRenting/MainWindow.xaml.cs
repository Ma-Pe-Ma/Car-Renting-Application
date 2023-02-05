using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CarRenting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CarRentingDBContext db = new CarRentingDBContext();

            // Note: This sample requires the database to be created before running.
            Console.WriteLine($"Database path: {db.DbPath}.");

            // Create
            Console.WriteLine("Inserting a new blog");

            Car car0 = new Car();
            car0.CarId = "AAA-111";
            car0.Manufacturer = "TOYOTA";
            car0.Model = "YARIS";
            car0.Year = 2000;

            Renting renting = new Renting();
            renting.StartTime = DateTime.Now.AddDays(1);
            renting.EndTime = DateTime.Now.AddDays(3);

            car0.Rentings.Add(renting);

            //db.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
            db.Add(car0);
            db.SaveChanges();

            // Read
            Console.WriteLine("Querying for a blog");
            var car = db.Cars
                .OrderBy(b => b.CarId)
                .First();

            Console.WriteLine("RENTING COUNT: " + car.Rentings.Count);

            // Update
            Console.WriteLine("Updating the blog and adding a post");

            db.Remove(car);
            db.SaveChanges();

            car.Model = "Supra";
            car.CarId = "ZZZ-999";
            car.Rentings.Add(
                new Renting { StartTime = DateTime.Now.AddDays(5), EndTime = DateTime.Now.AddDays(7) });

            db.Add(car);

            db.SaveChanges();

            var car2 = db.Cars
                .OrderBy(b => b.CarId)
                .First();

            Console.WriteLine("RENTING COUNT: " + car2.Rentings.Count);

            // Delete
            Console.WriteLine("Delete the blog");
            db.Remove(car);
            db.SaveChanges();

        }
    }
}
