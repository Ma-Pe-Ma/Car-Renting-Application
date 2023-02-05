using CarRenting.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentingTests
{
    [TestClass]
    public class CarRentingItemTest
    {
        [TestMethod]
        public void TestCarItem()
        {
            CarListViewModel carListViewModel = new CarListViewModel();

            Car car = new Car();
            car.LicensePlateNumber = "ABC-123";
            car.Manufacturer = "Toyota";
            car.Model = "Corolla";
            car.Year = 2012;

            carListViewModel.SelectedCar = new CarViewModel(car);

            Assert.IsTrue(carListViewModel.isValidUserCarInput(), "Car input should be valid.");
            
            car.LicensePlateNumber = "";
            Assert.IsFalse(carListViewModel.isValidUserCarInput(), "License plate number is invalid.");

            car.Year = 3000;
            car.LicensePlateNumber = "ABC-123";
            Assert.IsFalse(carListViewModel.isValidUserCarInput(), "Year is not valid.");            
        }

        [TestMethod]
        public void TestRentingItem()
        {
            RentingListViewModel rentingListViewModel = new RentingListViewModel();
            rentingListViewModel.RefreshCommand.Execute(null);

            rentingListViewModel.CarListViewModel = new CarListViewModel();
    
            Renting renting = new Renting();

            renting.CustomerName = "John Doe";
            renting.Car = new Car();
            renting.StartTime = DateTime.Now.Date;
            renting.EndTime = DateTime.Now.Date;

            RentingViewModel rentingViewModel = new RentingViewModel(renting);

            rentingListViewModel.SelectedRenting = rentingViewModel;
            Assert.IsTrue(rentingListViewModel.isValidUserRentingInput(), "Renting should be valid.");

            renting.EndTime = DateTime.Now.AddDays(-1).Date;
            Assert.IsFalse(rentingListViewModel.isValidUserRentingInput(), "Date interval should be invalid.");

            renting.EndTime = DateTime.Now.Date;
            renting.CustomerName = null;
            Assert.IsFalse(rentingListViewModel.isValidUserRentingInput(), "Customer name should be null!");
        }
    }
}
