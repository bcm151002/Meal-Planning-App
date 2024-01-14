using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Tema.ViewModel;

namespace Tema.View
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        private ObservableCollection<string> selectedIngredients = new ObservableCollection<string>();

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;

            if (listBox != null)
            {
                // Clear existing selection
                selectedIngredients.Clear();

                // Add the selected items to the collection
                foreach (var selectedItem in listBox.SelectedItems)
                {
                    selectedIngredients.Add(selectedItem.ToString());
                }
            }
        }

        public Settings()
        {
            InitializeComponent();

        }

    }
}
