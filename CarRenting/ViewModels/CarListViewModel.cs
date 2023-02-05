using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CarRenting.ViewModels
{       
    public class CarListViewModel : ViewModelBase
    {
        #region Constructors and collection properties

        DbSet<Renting> rentings;

        CarRentingDBContext dbContext;// = new CarRentingDBContext();

        public CarListViewModel() 
        {
            //CarRentingDBContext dBContext= new CarRentingDBContext();
            dbContext = new CarRentingDBContext();

            List<CarViewModel> carViewModels = new List<CarViewModel>();

            foreach (Car car in dbContext.Cars)
            {
                carViewModels.Add(new CarViewModel(car) { Saved = true});
            }

            rentings = dbContext.Rentings;

            this.AllCars = new ObservableCollection<CarViewModel>(carViewModels);
            this.AllCars.CollectionChanged += this.OnCollectionChanged;
        }

        public CarListViewModel(DbSet<Car> cars)
        {
            List<CarViewModel> carViewModels = new List<CarViewModel>();

            foreach (Car car in cars)
            {
                carViewModels.Add(new CarViewModel(car) { Saved = true });
            }

            this.AllCars = new ObservableCollection<CarViewModel>(carViewModels);
            this.AllCars.CollectionChanged += this.OnCollectionChanged;
        }

        public ObservableCollection<CarViewModel> AllCars { get; private set; }

        void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (CarViewModel carVM in e.NewItems)
                    carVM.PropertyChanged += this.OnCarViewModelPropertyChanged;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (CarViewModel carVM in e.OldItems)
                    carVM.PropertyChanged -= this.OnCarViewModelPropertyChanged;
        }

        void OnCarViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {

        }

        #endregion

        #region Properties

        CarViewModel _selectedCar;

        public CarViewModel SelectedCar
        {
            get { return _selectedCar; }
            set
            {
                if (_selectedCar == value)
                {
                    return;
                }

                _selectedCar = value;
                OnPropertyChanged(nameof(HasSelected));
            }
        }

        public bool HasSelected
        {
            get { return _selectedCar != null; }
        }

        string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if (_errorMessage == value) return;

                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        #endregion

        #region Commands

        private void SaveUpdate()
        {
            // Check if a car is selected or not
            if (_selectedCar != null)
            {
                if (!isValidUserCarInput())
                {
                    return;
                }

                //do not save car if new license plate number is the same as an other car's
                foreach (Car car in dbContext.Cars)  
                {
                    //skip to next item if the selected car is the same as the checked
                    if (car.CarId == _selectedCar._car.CarId)
                    {
                        continue;
                    }

                    // if car exists with same license plate do not save the edited one
                    if (car.LicensePlateNumber == _selectedCar._car.LicensePlateNumber)
                    {
                        string invalidLicensePlateNumber = "License plate already used.";
                        SetMessage(invalidLicensePlateNumber);
                        return;
                    }
                }

                if (!dbContext.Cars.Contains(_selectedCar._car))
                {
                    dbContext.Cars.Add(_selectedCar._car);
                }
                else
                {
                    dbContext.Update(_selectedCar._car);
                }

                dbContext.SaveChanges();
                _selectedCar.Saved = true;
            }
        }

        private bool CanSave()
        {
            return true;
        }

        private void AddNew()
        {
            Car newCar = new Car();
            newCar.Year = DateTime.Now.Year;
            CarViewModel carViewModel = new CarViewModel(newCar);
            carViewModel.Saved = false;

            AllCars.Add(carViewModel);
            //base.OnPropertyChanged(nameof(CarViewModel));
        }

        private bool CanAdd()
        {
            return true;
        }

        private void DeleteSelected()
        {
            if (_selectedCar != null)
            {
                if (dbContext.Cars.Contains(_selectedCar._car))
                {
                    dbContext.Remove(_selectedCar._car);
                    dbContext.SaveChanges();
                }

                AllCars.Remove(_selectedCar);
            }
        }

        private bool CanDelete()
        {
            return true;
        }

        Command _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new Command(param => this.SaveUpdate(), param => this.CanSave());
                }
                return _saveCommand;
            }
        }

        Command _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new Command(param => this.DeleteSelected(), param => this.CanDelete());
                }
                return _deleteCommand;
            }
        }

        Command _addCommand;
        public ICommand AddCommand
        {
            get
            {            
                if (_addCommand == null)
                {
                    _addCommand = new Command(param => this.AddNew(), param => this.CanAdd());
                }
                return _addCommand;
            }
        }

        #endregion

        #region Helper methods
        public CarViewModel? GetCarViewByCar(Car car)
        {
            foreach (CarViewModel carViewModel in this.AllCars)
            {
                if (carViewModel._car == car)
                {
                    return carViewModel;
                }
            }

            return null;
        }

        public bool isValidUserCarInput()
        {
            if (String.IsNullOrEmpty(_selectedCar._car.LicensePlateNumber))
            {
                string noLicensePlate = "License plate not specified.";
                SetMessage(noLicensePlate);
                return false;
            }

            if (String.IsNullOrEmpty(_selectedCar._car.Manufacturer))
            {
                string noManufacturer = "Manufacturer not specified.";
                SetMessage(noManufacturer);
                return false;
            }

            if (String.IsNullOrEmpty(_selectedCar._car.Model))
            {
                string noModel = "Model not specified.";
                SetMessage(noModel);
                return false;
            }

            if (_selectedCar.Year < 1950 || _selectedCar.Year > DateTime.Now.Year)
            {
                string invalidYear = "Invalid year given.";
                SetMessage(invalidYear);
                return false;
            }

            return true;
        }

        DateTime lastTime = DateTime.Now;
        const int messageTimeoutInSecs = 3;
        public async Task SetMessage(string errorDescription)
        {
            ErrorMessage = errorDescription;

            lastTime = DateTime.Now;

            await Task.Delay(messageTimeoutInSecs * 1000);

            if (DateTime.Now >= lastTime.AddMilliseconds(messageTimeoutInSecs * 1000))
            {
                ErrorMessage = "";
            }
        }

        #endregion
    }
}
