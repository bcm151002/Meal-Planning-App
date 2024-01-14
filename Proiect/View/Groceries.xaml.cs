using System;
using System.Collections.Generic;
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
using Tema.Database;
using Tema.Utilities;

namespace Tema.View
{
    public partial class Groceries : UserControl
    {
        public Groceries()
        {
            InitializeComponent();
            List<Ingredients> allIngredients = IngredientsHistory.Instance.GetIngredients();
            recipesDataGrid.ItemsSource = allIngredients;

            decimal totalPrice = IngredientsHistory.Instance.GetTotalPrice();
            totalPriceTextBlock.Text = $"Total Price: {totalPrice.ToString("C2")}";
        }
        private void recipesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

    }
}
