using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRenting.ViewModels
{
    public class CarViewModel : ViewModelBase
    {
        public readonly Car _car;
        public CarViewModel(Car car)
        {
            _car = car;
        }

        #region Properties

        bool _saved = false;
        public bool Saved
        {
            get { return _saved;}

            set { 
                if(_saved == value) return;

                _saved = value; 
                OnPropertyChanged(nameof(Saved));
            }
        }

        public string LicensePlateNumber
        {
            get { 
                return _car.LicensePlateNumber; }
            set
            {
                _car.LicensePlateNumber = value;
                base.OnPropertyChanged(nameof(LicensePlateNumber));
                Saved = false;
            }
        }

        public string Manufacturer
        {
            get { return _car.Manufacturer; }

            set
            {
                _car.Manufacturer = value;
                base.OnPropertyChanged(nameof(Manufacturer));
                Saved = false;
            }
        }

        public string Model
        {
            get { return _car.Model; }

            set
            {
                _car.Model = value;
                base.OnPropertyChanged(nameof(Model));
                Saved = false;
            }
        }

        public int Year
        {
            get { return _car.Year; }

            set
            {
                _car.Year = value;
                base.OnPropertyChanged(nameof(Year));
                Saved = false;
            }
        }

        public String CarProperties
        {
            get { 
                return _car.LicensePlateNumber + ", " + _car.Manufacturer + " - " + _car.Model;
            }
        }

        #endregion
    }
}
