using CarRenting.Views;
using CarRenting.ViewModels;

namespace CarRentingTests
{
    [TestClass]
    public class CarRentingListViewTest
    {
        [TestMethod]
        public void TestCarListViewModellListOperations()
        {
            CarListViewModel carListViewModel = new CarListViewModel();

            int originalListSize = carListViewModel.AllCars.Count();

            carListViewModel.AddCommand.Execute(null);

            Assert.AreEqual(carListViewModel.AllCars.Count(), (originalListSize + 1), "List element number should be bigger by one than original.");

            carListViewModel.SelectedCar = carListViewModel.AllCars.Last();

            carListViewModel.DeleteCommand.Execute(null);

            Assert.AreEqual(carListViewModel.AllCars.Count(), originalListSize, "List element number should be the same as original.");
        }

        [TestMethod]
        public void TestRentingListViewModellListOperations()
        {
            RentingListViewModel rentingListViewModel = new RentingListViewModel();
            rentingListViewModel.RefreshCommand.Execute(null);

            int originalListSize = rentingListViewModel.AllRentings.Count();

            rentingListViewModel.AddCommand.Execute(null);

            Assert.AreEqual(rentingListViewModel.AllRentings.Count(), (originalListSize + 1), "List element number should be bigger by one than original.");

            rentingListViewModel.SelectedRenting = rentingListViewModel.AllRentings.Last();

            rentingListViewModel.DeleteCommand.Execute(null);

            Assert.AreEqual(rentingListViewModel.AllRentings.Count(), originalListSize, "List element number should be the same as original.");
        }
    }
}