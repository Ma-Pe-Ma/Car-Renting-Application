using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRenting.ViewModels
{
    public class RentingViewModel : ViewModelBase
    {
        readonly public Renting _renting;

        public RentingViewModel(Renting renting)
        {
            _renting = renting;
        }

        #region Properties
        public DateTime StartTime
        {
            get { return _renting.StartTime; }
            set 
            {                 
                _renting.StartTime = value;
                base.OnPropertyChanged(nameof(StartTime));
                Saved = false;
            }
        }
        public DateTime EndTime
        {
            get { return _renting.EndTime; }
            set
            {
                _renting.EndTime = value;
                base.OnPropertyChanged(nameof(EndTime));
                Saved = false;
            }
        }

        public Car Car
        {
            get { return _renting.Car; }

            set
            {
                if (_renting.Car == value) { return; }
                _renting.Car = value;
                base.OnPropertyChanged(nameof(Car));
                base.OnPropertyChanged(nameof(CarProperties));
                Saved = false;
            }
        }

        public string CustomerName
        {
            get { return _renting.CustomerName; }
            set
            {
                if (_renting.CustomerName == value) { return; }

                _renting.CustomerName = value;
                base.OnPropertyChanged(nameof(CustomerName));
                Saved = false;
            }
        }

        public string CarProperties
        {
            get { 
                if (_renting.Car == null)
                {
                    return "*Unselected*";
                }

               return _renting.Car.LicensePlateNumber + ", " + _renting.Car.Manufacturer + " - " + _renting.Car.Model;
            }
        }

        bool _saved = false;
        public bool Saved
        {
            get { return _saved; }

            set
            {
                if (_saved == value) return;

                _saved = value;
                OnPropertyChanged(nameof(Saved));
            }
        }
    }

    #endregion
}
