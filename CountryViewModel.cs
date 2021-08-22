using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_MVVM_Tab_ListView_EFCore.Models;

namespace WPF_MVVM_Tab_ListView_EFCore
{
    public class CountryViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private string countryName;
        public string CountryName
        {
            get { return countryName; }
            set
            {
                countryName = value;
                NotifyPropertyChanged("CountryName");
            }
        }

        private string greeting;
        public string Greeting
        {
            get { return greeting; }
            set
            {
                greeting = value;
                NotifyPropertyChanged("Greeting");
            }
        }
        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                NotifyPropertyChanged("ErrorMessage");
            }
        }

        public List<TblCountry> countryList;
        public List<TblCountry> CountryList
        {
            get { return countryList; }
            set
            {
                countryList = value;
                NotifyPropertyChanged("CountryList");
            }
        }

        private TblCountry selectedCountry;
        public TblCountry SelectedCountry
        {
            get { return selectedCountry; }
            set
            {
                selectedCountry = value;
                NotifyPropertyChanged("SelectedCountry");
            }
        }
        public ICommand cmdSave { get; private set; }
        public bool CanExecute 
        {
            get { return !string.IsNullOrEmpty(CountryName) & !string.IsNullOrEmpty(Greeting); }
        }

        public bool CanDelete 
        {
            get { return SelectedCountry != null; }
        }

        public ICommand cmdDeleteItem { get; private set; }
        public CountryViewModel()
        {
            cmdSave = new RelayCommand(Save, () => CanExecute);
            cmdDeleteItem = new RelayCommand(Delete, () => CanDelete);

            using (var context = new WPF_DBContext())
            {
                CountryList = context.TblCountries.ToList();
            }
        }

        private void Delete()
        {
            using (var context = new WPF_DBContext())
            {
                context.TblCountries.Remove(SelectedCountry);
                context.SaveChanges();
                CountryList = context.TblCountries.ToList();
            }
        }
        private void Save()
        {
            try
            {
                using(var context = new WPF_DBContext())
                {
                    var country = new TblCountry()
                    {
                        CountryName = CountryName,
                        Greeting = Greeting
                    };
                    context.Add(country);
                    context.SaveChanges();
                    CountryList = context.TblCountries.ToList();
                }
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            ErrorMessage = "Data saved successfully!";
        }
    }
}
