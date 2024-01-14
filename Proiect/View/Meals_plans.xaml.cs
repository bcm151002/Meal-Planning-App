using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;
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
using Tema.Utilities;

namespace Tema.View
{
    public partial class Meals_plans : UserControl
    {
        public Meals_plans()
        {
            InitializeComponent();
            recipesDataGrid.ItemsSource = getMeals();
        }
        public ObservableCollection<Member3> getMeals()
        {
            var converter = new BrushConverter();
            ObservableCollection<Member3> Member3s = new ObservableCollection<Member3>();

            var context = DataSharing.Instance.Context;
            var currentUsername = DataSharing.Instance.Username;
            var currentUserID = context.Users.FirstOrDefault(u => u.username == currentUsername)?.user_id;

            if (currentUserID.HasValue)
            {
                var mealIdsWithFavorites = context.Meals_recipes
                    .Where(mr => context.Users_recipes
                        .Any(ur => ur.user_id == currentUserID && ur.favorite && ur.recipe_id == mr.recipe_id))
                    .Select(mr => mr.meal_id)
                    .Distinct()
                    .ToList();

                foreach (var mealId in mealIdsWithFavorites)
                {
                    var meal = context.Meals.FirstOrDefault(m => m.meal_id == mealId);

                    if (meal != null)
                    {
                        var usersWithFavoriteRecipesCount = context.Users_recipes
                            .Where(ur => ur.favorite && context.Meals_recipes
                                .Any(mr => mr.meal_id == mealId && mr.recipe_id == ur.recipe_id))
                            .Select(ur => ur.user_id)
                            .Distinct()
                            .Count();

                        string mealType = (char.ToUpper(meal.meal_type[0])).ToString();
                        Color color = (Color)ColorConverter.ConvertFromString("#e6d707");

                        Brush colorB = Brushes.Gray;
                        if (mealType[0] == 'B') { colorB = Brushes.Blue; }
                        if (mealType[0] == 'L') { colorB = new SolidColorBrush(color); }
                        if (mealType[0] == 'D') { colorB = Brushes.Violet; }

                        Member3s.Add(new Member3
                        {
                            mealId = meal.meal_id.ToString(),
                            meal_type = mealType,
                            data = meal.date.ToString("yyyy-MM-dd"),
                            views = "   " + usersWithFavoriteRecipesCount.ToString(),
                            BgColor = colorB
                        });
                    }
                }
            }

            return Member3s;
        }
        private void recipesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public RecipeDetails getRecipeDetails(int recipeId)
        {
            var context = DataSharing.Instance.Context;

            var recipeDetails = context.Recipes
                .Where(r => r.recipe_id == recipeId)
                .Select(r => new RecipeDetails
                {
                    RecipeId = r.recipe_id,
                    RecipeName = r.recipe_name,
                    Description = r.description,
                    Instructions = r.instructions
                })
                .FirstOrDefault();

            return recipeDetails;
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
        private void infoButtonClick(object sender, RoutedEventArgs e)
        {
            var context = DataSharing.Instance.Context;
            Button button = (Button)sender;

            DataGridRow obj = FindParent<DataGridRow>(button);
            var data = obj.Item;
            int mealId = int.Parse(data.GetType().GetProperty("mealId").GetValue(data, null).ToString());

            StringBuilder messageBuilder = new StringBuilder();

            var recipesInMeal = context.Meals_recipes
                .Where(mr => mr.meal_id == mealId)
                .Select(mr => mr.Recipe)
                .ToList();


            foreach (var recipe in recipesInMeal)
            {
                messageBuilder.AppendLine($"Recipe Name: {recipe.recipe_name}");
                messageBuilder.AppendLine($"Description: {recipe.description}");
                messageBuilder.AppendLine($"Instructions: {recipe.instructions}");
                messageBuilder.AppendLine();
            }

            var customMessageBox = new CustomMessageBox2002(messageBuilder.ToString(), $"Meal Plan Details for Meal ID {mealId}");
            customMessageBox.ShowDialog();
        }
        private void buyButtonClick(object sender, RoutedEventArgs e)
        {
            var context = DataSharing.Instance.Context;
            Button button = (Button)sender;

            DataGridRow obj = FindParent<DataGridRow>(button);
            var data = obj.Item;
            int mealId = int.Parse(data.GetType().GetProperty("mealId").GetValue(data, null).ToString());

            var recipesInMeal = context.Meals_recipes
                .Where(mr => mr.meal_id == mealId)
                .Select(mr => mr.Recipe)
                .ToList();

            foreach (var recipe in recipesInMeal)
            {
                var recipeIngredients = context.Recipes_Ingredients
                    .Where(ri => ri.recipe_id == recipe.recipe_id)
                    .Select(ri => new
                    {
                        IngredientId = ri.ingredient_id,
                        Quantity = ri.quantity
                    })
                    .ToList();

                foreach (var recipeIngredient in recipeIngredients)
                {
                    var ingredient = context.Ingredients.FirstOrDefault(i => i.ingredient_id == recipeIngredient.IngredientId);

                    if (ingredient != null)
                    {
                        IngredientsHistory.Instance.Add(ingredient.ingredient_id, ingredient.ingredient_name, ingredient.category, ingredient.price, recipeIngredient.Quantity);
                    }
                }
            }
        }
        public class Member3
        {
            public string meal_type { get; set; }
            public string mealId { get; set; }
            public string data { get; set; }
            public string views { get; set; }
            public Brush BgColor { get; set; }
        }
        public class RecipeDetails
        {
            public int RecipeId { get; set; }
            public string RecipeName { get; set; }
            public string Description { get; set; }
            public string Instructions { get; set; }
        }

        public class CustomMessageBox2002 : Window
        {
            public CustomMessageBox2002(string message, string title)
            {
                Title = title;
                Width = 400;
                MaxHeight = SystemParameters.WorkArea.Height * 0.8;
                WindowStartupLocation = WindowStartupLocation.CenterScreen;
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#212529"));

                var textBlock = new TextBlock
                {
                    Text = message,
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = Brushes.White,
                    Margin = new Thickness(10)
                };

                var scrollViewer = new ScrollViewer
                {
                    Content = textBlock,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Auto
                };

                var closeButton = new Button
                {
                    Content = "OK",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Margin = new Thickness(0, 10, 0, 10),
                    Padding = new Thickness(10),
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4e565d")),
                    Foreground = Brushes.White
                };

                closeButton.Click += (sender, e) => Close();

                var grid = new Grid();
                grid.Children.Add(scrollViewer);
                grid.Children.Add(closeButton);

                Content = grid;
            }
        }
    }
}
