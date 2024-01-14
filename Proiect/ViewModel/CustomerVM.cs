using System;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Windows;
using System.Windows.Input;
using Tema.Database;
using Tema.Model;
using Tema.Utilities;
using Tema.View;

namespace Tema.ViewModel
{
    class CustomerVM : ViewModelBase
    {
        private readonly PageModel _pageModel;

        public int CustomerID
        {
            get { return _pageModel.CustomerCount; }
            set { _pageModel.CustomerCount = value; OnPropertyChanged(); }
        }

        public string CustomerUsername
        {
            get { return _pageModel.CustomerUsername; }
            set { _pageModel.CustomerUsername = value; OnPropertyChanged(); }
        }

        public string CustomerAccType
        {
            get { return _pageModel.CustomerAccType; }
            set { _pageModel.CustomerAccType = value; OnPropertyChanged(); }
        }

        public string CustomerUsernameNou
        {
            get { return _pageModel.CustomerUsernameNou; }
            set { _pageModel.CustomerUsernameNou = value; OnPropertyChanged(); }
        }

        public string CustomerParolaNoua
        {
            get { return _pageModel.CustomerParolaNoua; }
            set { _pageModel.CustomerParolaNoua = value; OnPropertyChanged(); }
        }

        public string CustomerEmailNou
        {
            get { return _pageModel.CustomerEmailNou; }
            set { _pageModel.CustomerEmailNou = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; private set; }

        public ICommand DeleteAccountCommand { get; private set; }

        private void SaveExecute(object parameter)
        {
            try
            {
                var currentUser = DataSharing.Instance.Context.Users.FirstOrDefault(u => u.username == DataSharing.Instance.Username);

                if (currentUser != null)
                {
                    if (!string.IsNullOrEmpty(CustomerUsernameNou))
                    {
                        var existingUser = DataSharing.Instance.Context.Users.FirstOrDefault(u => u.username == CustomerUsernameNou);

                        if (existingUser == null)
                        {
                            currentUser.username = CustomerUsernameNou;
                            CustomerUsername = CustomerUsernameNou;
                            DataSharing.Instance.Username = CustomerUsernameNou; // Actualizați și username-ul în DataSharing
                        }
                        else
                        {
                            MessageBox.Show("Username-ul introdus există deja. Alegeți alt username.");
                            return;
                        }
                    }

                    if (!string.IsNullOrEmpty(CustomerParolaNoua))
                    {
                        currentUser.password = CustomerParolaNoua;
                    }

                    if (!string.IsNullOrEmpty(CustomerEmailNou))
                    {
                        currentUser.email = CustomerEmailNou;
                    }

                    DataSharing.Instance.Context.SubmitChanges();

                    MessageBox.Show("Datele au fost actualizate cu succes.");
                }
            }
            catch (Exception ex)
            {
                // Tratarea excepțiilor aici
                MessageBox.Show($"A apărut o eroare: {ex.Message}");
            }
        }


        private void DeleteAccountExecute(object parameter)
        {
            try
            {
                var currentUser = DataSharing.Instance.Context.Users.FirstOrDefault(u => u.username == DataSharing.Instance.Username);

                if (currentUser != null)
                {
                    DataSharing.Instance.Context.Users.DeleteOnSubmit(currentUser);
                    DataSharing.Instance.Context.SubmitChanges();

                    MessageBox.Show("Contul a fost șters cu succes.");

                    // Închideți aplicația după ștergere
                    Application.Current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                // Tratarea excepțiilor aici
                MessageBox.Show($"A apărut o eroare: {ex.Message}");
            }
        }

        public CustomerVM()
        {
            _pageModel = new PageModel();

            string username = DataSharing.Instance.Username;

            using (var context = new dbDataContext())
            {
                var user = (from u in context.Users
                            where u.username == username
                            select u).SingleOrDefault();

                if (user != null)
                {
                    CustomerID = user.user_id;
                    CustomerUsername = user.username;
                    CustomerAccType = user.role;
                }
            }

            // Inițializează comenzile în constructor
            SaveCommand = new RelayCommand(SaveExecute);
            DeleteAccountCommand = new RelayCommand(DeleteAccountExecute);

            
        }



    }
}
