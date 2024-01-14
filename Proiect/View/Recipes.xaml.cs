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
using Tema.Database;
using Tema.Utilities;

namespace Tema.View
{
    public partial class Recipes : UserControl
    {
        public Recipes()
        {
            InitializeComponent();
            recipesDataGrid.ItemsSource = getFavMembers();
        }
        public ObservableCollection<Member2> getFavMembers()
        {
            var converter = new BrushConverter();
            ObservableCollection<Member2> Member2s = new ObservableCollection<Member2>();

            var context = DataSharing.Instance.Context;
            var currentUsername = DataSharing.Instance.Username;
            var currentUserID = context.Users.FirstOrDefault(u => u.username == currentUsername).user_id;

            var userFavorites = context.Users_recipes
                .Where(ur => ur.user_id == currentUserID && ur.favorite)
                .Select(ur => ur.recipe_id)
                .ToList();

            foreach (var recipeId in userFavorites)
            {
                var recipe = context.Recipes.FirstOrDefault(r => r.recipe_id == recipeId);

                if (recipe != null)
                {
                    var nutritionInfo = context.Nutrition_Informations
                        .Where(ni => ni.recipe_id == recipe.recipe_id)
                        .FirstOrDefault();

                    var rating = context.Users_recipes
                        .Count(ur => ur.recipe_id == recipe.recipe_id && ur.favorite);

                    string Srating = rating.ToString() + "/" + context.Users.Count().ToString();

                    Brush colorB = Brushes.Gray;
                    if (nutritionInfo.calories <= 500) { colorB = Brushes.Green; }
                    if (nutritionInfo.calories > 500 && nutritionInfo.calories <= 1000) { colorB = Brushes.Orange; }
                    if (nutritionInfo.calories > 1000) { colorB = Brushes.Red; }

                    Member2s.Add(new Member2
                    {
                        receipeId = recipe.recipe_id.ToString(),
                        receipeName = recipe.recipe_name,
                        cockTime = recipe.preparation_time.ToString() + " minutes",
                        rating = Srating,
                        kcalBalance = (nutritionInfo != null) ? nutritionInfo.calories.ToString() : "N/A",
                        BgColor = colorB,
                        fav = true
                    });
                }
            }

            return Member2s;
        }

        private static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;
            if (parentObject is T parent)
                return parent;
            else
                return FindParent<T>(parentObject);
        }

        private void recipesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void deleteButtonClick(object sender, RoutedEventArgs e)
        {
            var context = DataSharing.Instance.Context;
            var currentUsername = DataSharing.Instance.Username;
            Button button = (Button)sender;

            DataGridRow obj = FindParent<DataGridRow>(button);
            var data = obj.Item;
            var currentUserID = context.Users.FirstOrDefault(u => u.username == currentUsername).user_id;

            int receipeID = int.Parse(data.GetType().GetProperty("receipeId").GetValue(data, null).ToString());
            var userRecipe = context.Users_recipes
                .FirstOrDefault(ur => ur.recipe_id == receipeID && ur.user_id == currentUserID);

            userRecipe.favorite = false;

            context.SubmitChanges();
            var items = recipesDataGrid.ItemsSource as ObservableCollection<Member2>;
            items.Clear();
            recipesDataGrid.ItemsSource = getFavMembers();
            recipesDataGrid.Items.Refresh();
        }

        private void buyButtonClick(object sender, RoutedEventArgs e)
        {
            var context = DataSharing.Instance.Context;
            Button button = (Button)sender;

            DataGridRow obj = FindParent<DataGridRow>(button);
            var data = obj.Item;
            int receipeId = int.Parse(data.GetType().GetProperty("receipeId").GetValue(data, null).ToString());

            var recipeIngredients = context.Recipes_Ingredients
                .Where(ri => ri.recipe_id == receipeId)
                .ToList();

            List<Ingredients> ingredientsForRecipe = new List<Ingredients>();

            foreach (var recipeIngredient in recipeIngredients)
            {
                var ingredient = context.Ingredients.FirstOrDefault(i => i.ingredient_id == recipeIngredient.ingredient_id);
                IngredientsHistory.Instance.Add(ingredient.ingredient_id, ingredient.ingredient_name, ingredient.category, ingredient.price, recipeIngredient.quantity);
            }
        }
    }
}    
    public class Member2
    {
        public string receipeId { get; set; }
        public string receipeName { get; set; }
        public string cockTime { get; set; }
        public string rating { get; set; }
        public string kcalBalance { get; set; }
        public Brush BgColor { get; set; }
        public bool fav { get; set; }
    }
