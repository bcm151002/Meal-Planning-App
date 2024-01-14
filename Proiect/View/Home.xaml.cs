using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tema.Database;
using Tema.Utilities;
using static Tema.View.Meals_plans;

namespace Tema.View
{
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
            recipesDataGrid.ItemsSource = getMembers();
        }

        public ObservableCollection<Member> getMembers()
        {
            var converter = new BrushConverter();
            ObservableCollection<Member> members = new ObservableCollection<Member>();

            var context = DataSharing.Instance.Context;
            var currentUsername = DataSharing.Instance.Username;
            var currentUser = context.Users.FirstOrDefault(u => u.username == currentUsername);

            foreach (var recipe in context.Recipes)
            {
                var nutritionInfo = context.Nutrition_Informations
                    .Where(ni => ni.recipe_id == recipe.recipe_id)
                    .FirstOrDefault();

                var rating = context.Users_recipes
                    .Count(ur => ur.recipe_id == recipe.recipe_id && ur.favorite);

                bool favorit = context.Users_recipes
                    .Any(ur => ur.recipe_id == recipe.recipe_id && ur.user_id == currentUser.user_id && ur.favorite);

                string Srating = rating.ToString() + "/" + context.Users.Count().ToString();

                Brush colorB = Brushes.Gray;
                if (nutritionInfo.calories <= 500) { colorB = Brushes.Green; }
                if (nutritionInfo.calories > 500 && nutritionInfo.calories <= 1000) { colorB = Brushes.Orange; }
                if (nutritionInfo.calories > 1000) { colorB = Brushes.Red; }

                members.Add(new Member
                {
                    receipeId = recipe.recipe_id.ToString(),
                    receipeName = recipe.recipe_name,
                    cockTime = recipe.preparation_time.ToString() + " minutes",
                    rating = Srating,
                    kcalBalance = (nutritionInfo != null) ? nutritionInfo.calories.ToString() : "N/A",
                    BgColor = colorB,
                    fav = favorit
                });
            }

            return members;
        }

        public ObservableCollection<Member> GetMembersByName(string searchString)
        {
            var converter = new BrushConverter();
            ObservableCollection<Member> filteredMembers = new ObservableCollection<Member>();

            var context = DataSharing.Instance.Context;
            var currentUsername = DataSharing.Instance.Username;
            var currentUser = context.Users.FirstOrDefault(u => u.username == currentUsername);

            foreach (var recipe in context.Recipes.Where(r => r.recipe_name.Contains(searchString)))
            {
                var nutritionInfo = context.Nutrition_Informations
                    .Where(ni => ni.recipe_id == recipe.recipe_id)
                    .FirstOrDefault();

                var rating = context.Users_recipes
                    .Count(ur => ur.recipe_id == recipe.recipe_id && ur.favorite);

                bool favorit = context.Users_recipes
                    .Any(ur => ur.recipe_id == recipe.recipe_id && ur.user_id == currentUser.user_id && ur.favorite);

                string Srating = rating.ToString() + "/" + context.Users.Count().ToString();

                Brush colorB = Brushes.Gray;
                if (nutritionInfo.calories <= 500) { colorB = Brushes.Green; }
                if (nutritionInfo.calories > 500 && nutritionInfo.calories <= 1000) { colorB = Brushes.Orange; }
                if (nutritionInfo.calories > 1000) { colorB = Brushes.Red; }

                filteredMembers.Add(new Member
                {
                    receipeId = recipe.recipe_id.ToString(),
                    receipeName = recipe.recipe_name,
                    cockTime = recipe.preparation_time.ToString() + " minutes",
                    rating = Srating,
                    kcalBalance = (nutritionInfo != null) ? nutritionInfo.calories.ToString() : "N/A",
                    BgColor = colorB,
                    fav = favorit
                });
            }

            return filteredMembers;
        }

        private void recipesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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

        private void favoriteButtonClick(object sender, RoutedEventArgs e)
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

            if (userRecipe != null)
            {
                userRecipe.favorite = !userRecipe.favorite;
            }
            else
            {
                var newUserRecipe = new Users_recipe
                {
                    user_id = currentUserID,
                    recipe_id = receipeID,
                    favorite = true
                }; 

                context.Users_recipes.InsertOnSubmit(newUserRecipe);
            }

            context.SubmitChanges();
            var items = recipesDataGrid.ItemsSource as ObservableCollection<Member>; ;
            items.Clear();
            recipesDataGrid.ItemsSource = getMembers();
            recipesDataGrid.Items.Refresh();
        }

        private Recipe getRecipeDetails(int receipeID)
        {
            var context = DataSharing.Instance.Context;
            var recipeDetails = context.Recipes
                .Where(r => r.recipe_id == receipeID)
                .FirstOrDefault();

            return recipeDetails;
        }

        private void receipeDetailesButton(object sender, RoutedEventArgs e)
        {
            var context = DataSharing.Instance.Context;
            Button button = (Button)sender;

            DataGridRow obj = FindParent<DataGridRow>(button);
            var data = obj.Item;
            int receipeID = int.Parse(data.GetType().GetProperty("receipeId").GetValue(data, null).ToString());

            var recipeDetails = getRecipeDetails(receipeID);

            string message = $"Recipe Name: {recipeDetails.recipe_name}\n";
            message += $"Description: {recipeDetails.description}\n";
            message += $"Instructions: {recipeDetails.instructions}";

            var customMessageBox = new CustomMessageBox2002(message, $"Meal Plan Details for Meal ID {receipeID}");
            customMessageBox.ShowDialog();
        }

        private void searchClickButton(object sender, RoutedEventArgs e)
        {
            string filter_text = txtSearch.Text;
            
            var items = recipesDataGrid.ItemsSource as ObservableCollection<Member>; ;
            items.Clear();
            recipesDataGrid.ItemsSource = GetMembersByName(filter_text);
            recipesDataGrid.Items.Refresh();
        }
    }

    public class Member
    {
        public string receipeId { get; set; }
        public string receipeName { get; set; }
        public string cockTime { get; set; }
        public string rating { get; set; }
        public string kcalBalance { get; set; }
        public Brush BgColor { get; set; }
        public bool fav { get; set; }
    }

}
