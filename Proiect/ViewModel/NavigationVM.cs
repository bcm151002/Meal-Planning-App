using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tema.Utilities;
using System.Windows.Input;
using Tema.Properties;
using System.Windows.Controls;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Tema.ViewModel
{
     class NavigationVM : Utilities.ViewModelBase
    {
        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }

        }

        private bool _isAdminButtonVisible;
        public bool IsAdminButtonVisible
        {
            get { return _isAdminButtonVisible; }
            set { _isAdminButtonVisible = value; OnPropertyChanged(); }
        }

        public ICommand HomeCommand { get; set; }
        public ICommand CustomersCommand {  get; set; }
        public ICommand GroceriesCommand { get; set; }
        public ICommand Meals_plansCommand { get; set; }
        public ICommand RecipesCommand { get; set; }
        public ICommand TrackingCommand { get; set; }
        public ICommand SettingsCommand { get; set; }

        private void Home(object obj) => CurrentView = new HomeVM();
        private void Customer(object obj) => CurrentView = new CustomerVM();
        private void Groceries(object obj) => CurrentView = new GroceriesVM();
        private void Meals_plans(object obj) => CurrentView = new Meals_plansVM();
        private void Recipes(object obj) => CurrentView = new RecipesVM();
        private void Tracking(object obj) => CurrentView = new TrackingVM();
        private void Setting(object obj) => CurrentView = new SettingVM();

        private bool CheckIfUserIsAdmin()
        {
            string username = DataSharing.Instance.Username;

            var user = DataSharing.Instance.Context.Users.FirstOrDefault(u => u.username == username);

            if (user != null && user.role == "admin")
            {
                return true;
            }

            return false;
        }

        public NavigationVM()
        {
            HomeCommand = new RelayCommand(Home);
            CustomersCommand = new RelayCommand(Customer);
            GroceriesCommand = new RelayCommand(Groceries);
            Meals_plansCommand = new RelayCommand(Meals_plans);
            RecipesCommand = new RelayCommand(Recipes);
            TrackingCommand = new RelayCommand(Tracking);
            SettingsCommand = new RelayCommand(Setting);

            IsAdminButtonVisible = CheckIfUserIsAdmin();

            CurrentView = new HomeVM();
        }

    }
}
