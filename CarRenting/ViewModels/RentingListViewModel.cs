using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CarRenting.ViewModels
{
    public class RentingListViewModel : ViewModelBase
    {
        #region Constructors and collection properties

        CarRentingDBContext dbContext; //= new CarRentingDBContext();

        public RentingListViewModel()
        {
            /*
            List<RentingViewModel> rentingViewModels = new List<RentingViewModel>();

            foreach (Renting renting in dBContext.Rentings)
            {
                rentingViewModels.Add(new RentingViewModel(renting) { Saved = true});
            }*/

            //AllRentings = new ObservableCollection<RentingViewModel>(rentingViewModels);
            AllRentings = new ObservableCollection<RentingViewModel>();
            AllRentings.CollectionChanged += OnCollectionChanged;

            //_carListViewModel = new CarListViewModel(dBContext.Cars);
        }

        public ObservableCollection<RentingViewModel> AllRentings { get; private set; }

        void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (RentingViewModel rentingVM in e.NewItems)
                    rentingVM.PropertyChanged += this.OnRentingViewModelPropertyChanged;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (RentingViewModel rentingVM in e.OldItems)
                    rentingVM.PropertyChanged -= this.OnRentingViewModelPropertyChanged;
        }

        void OnRentingViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {

        }

        #endregion

        #region Properties

        CarListViewModel _carListViewModel;
        public CarListViewModel CarListViewModel {
            get
            {
                return _carListViewModel;
            }
            set
            {
                _carListViewModel = value;
                OnPropertyChanged(nameof(CarListViewModel));
            }
        }

        CarViewModel _comboBoxCarViewModel;
        public CarViewModel ComboBoxCarViewModel
        {
            get { return _comboBoxCarViewModel; }
            set {
                _comboBoxCarViewModel = value;

                if (_comboBoxCarViewModel != null && _selectedRenting != null)
                {
                    _selectedRenting.Car = _comboBoxCarViewModel._car;
                }

                OnPropertyChanged(nameof(ComboBoxCarViewModel));
            }
        }

        RentingViewModel _selectedRenting;

        public RentingViewModel SelectedRenting
        {
            get { return _selectedRenting; }

            set
            {
                if (_selectedRenting == value) return;

                _selectedRenting = value;

                if (_selectedRenting != null)
                {
                    ComboBoxCarViewModel = CarListViewModel.GetCarViewByCar(_selectedRenting.Car);
                }

                OnPropertyChanged(nameof(HasSelected));
                OnPropertyChanged(nameof(SelectedRenting));
            }
        }

        public bool HasSelected
        {
            get { return _selectedRenting != null; }
        }

        string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set {
                if (_errorMessage == value) return;

                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        #endregion

        #region Commands

        private void SaveUpdate()
        {
            if (!IsValidUserRentingInput())
            {
                return;
            }

            if (OverlapsWithOtherRenting())
            {
                return;
            }

            if (dbContext.Rentings.Contains(_selectedRenting._renting))
            {
                dbContext.Update(_selectedRenting._renting);
            }
            else
            {
                _selectedRenting.Car.Rentings.Add(_selectedRenting._renting);
                dbContext.Update(_selectedRenting.Car);
            }

            dbContext.SaveChanges();
            _selectedRenting.Saved = true;
        }

        private bool CanSave()
        {
            return true;
        }

        private void DeleteSelected()
        { 
            if (_selectedRenting != null)
            {
                if (dbContext.Rentings.Contains(_selectedRenting._renting))
                {
                    dbContext.Remove(_selectedRenting._renting);
                    dbContext.SaveChanges();
                }
                
                AllRentings.Remove(_selectedRenting);
            }
        }

        private bool CanDelete()
        {
            return true;
        }

        private void AddNew()
        {
            Renting renting = new Renting();
            RentingViewModel rentingViewModel = new RentingViewModel(renting);
            renting.StartTime = DateTime.Now;
            renting.EndTime = DateTime.Now;

            AllRentings.Add(rentingViewModel);
        }

        private bool CanAdd()
        {
            return true;
        }

        private void Refresh()
        {
            dbContext = new CarRentingDBContext();

            List<RentingViewModel> rentingViewModels = new List<RentingViewModel>();

            AllRentings.Clear();

            foreach (Renting renting in dbContext.Rentings)
            {
                //rentingViewModels.Add(new RentingViewModel(renting) { Saved = true });

                AllRentings.Add(new RentingViewModel(renting) { Saved = true });
            }            
            
            //AllRentings.Aggregate(new ObservableCollection<RentingViewModel>(rentingViewModels));

            CarListViewModel = new CarListViewModel(dbContext.Cars);
        }

        private bool CanRefresh()
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

        Command _refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new Command(param => this.Refresh(), param => this.CanRefresh());
                }
                return _refreshCommand;
            }
        }

        #endregion

        #region Helper methods
        public bool IsValidUserRentingInput()
        {
            if (_selectedRenting == null)
            {
                string noRentingSelected = "No Renting selected.";
                SetMessage(noRentingSelected);
                return false;
            }

            if (_selectedRenting.Car == null)
            {
                //TODO: load strings from resource file
                string noCarSelectedTitle = "No car selected";
                string noCarSelectedDescription = "There's no car selected for the renting to save.";

                SetMessage(noCarSelectedDescription);
                //MessageBox.Show(noCarSelectedDescription, noCarSelectedTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (_selectedRenting.EndTime < _selectedRenting.StartTime)
            {
                //TODO: load strings from resource file
                string swappedDateTitle = "Swapped dates";
                string swappedDateDescription = "The renting's ending date is earlier than its start date.";
                //MessageBox.Show(swappedDateDescription, swappedDateTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                SetMessage(swappedDateDescription);

                return false;
            }

            if (String.IsNullOrEmpty(_selectedRenting.CustomerName))
            {
                //TODO: load strings from resource file
                string invalidCustomerTitle = "Invalid customer";
                string invalidCustomerDescription = "The customer name should not be empty.";
                //MessageBox.Show(swappedDateDescription, swappedDateTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                SetMessage(invalidCustomerDescription);

                return false;
            }

            return true;
        }

        bool OverlapsWithOtherRenting()
        {
            //TODO: load strings from resource file
            string overlappingDateTitle = "Overlapping dates";
            string overlappingDateDescription = "The renting's time interval overlaps with an other's.";

            foreach (Renting renting in _selectedRenting.Car.Rentings)
            {
                if (renting == _selectedRenting._renting || renting.RentingId == _selectedRenting._renting.RentingId)
                {
                    continue;
                }

                if (renting.StartTime.Date <= _selectedRenting.StartTime.Date && _selectedRenting.StartTime.Date <= renting.EndTime.Date)
                {
                    SetMessage(overlappingDateDescription);
                    //MessageBox.Show(overlappingDateDescription, overlappingDateTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                    return true;
                }

                if (renting.StartTime.Date <= _selectedRenting.EndTime.Date && _selectedRenting.EndTime.Date <= renting.EndTime.Date)
                {
                    SetMessage(overlappingDateDescription);
                    //MessageBox.Show(overlappingDateDescription, overlappingDateTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                    return true;
                }

                if (_selectedRenting.StartTime.Date <= renting.StartTime.Date && renting.StartTime.Date <= _selectedRenting.EndTime.Date)
                {
                    SetMessage(overlappingDateDescription);
                    //MessageBox.Show(overlappingDateDescription, overlappingDateTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                    return true;
                }
            }

            return false;
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