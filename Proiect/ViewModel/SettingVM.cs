using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Tema.Database;
using Tema.Model;
using Tema.Utilities;
using Tema.View;

namespace Tema.ViewModel
{
    class SettingVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;

        //adaugare cont
        public string SettingsContUsernameNouValue
        {
            get { return _pageModel.SettingsContUsernameNou; }
            set { _pageModel.SettingsContUsernameNou = value; OnPropertyChanged(); }
        }

        public string SettingsContParolaNouaValue
        {
            get { return _pageModel.SettingsContParolaNoua; }
            set { _pageModel.SettingsContParolaNoua = value; OnPropertyChanged(); }
        }

        public string SettingsContEmailNouValue
        {
            get { return _pageModel.SettingsContEmailNou; }
            set { _pageModel.SettingsContEmailNou = value; OnPropertyChanged(); }
        }
        public bool SettingsContValue
        {
            get { return _pageModel.SettingsContRole; }
            set { _pageModel.SettingsContRole = value; OnPropertyChanged(); }
        }



        //adaugare ingredient
        public string SettingsIngredienteNumeValue
        {
            get { return _pageModel.SettingsIngredienteNume; }
            set { _pageModel.SettingsIngredienteNume = value; OnPropertyChanged(); }
        }

        public string SettingsIngredienteCategorieValue
        {
            get { return _pageModel.SettingsIngredienteCategorie; }
            set { _pageModel.SettingsIngredienteCategorie = value; OnPropertyChanged(); }
        }

        public int SettingsIngredientePretValue
        {
            get { return _pageModel.SettingsIngredientePret; }
            set { _pageModel.SettingsIngredientePret = value; OnPropertyChanged(); }
        }



        //adaugare meal-plan

        public string SettingsMealsUsernameValue
        {
            get { return _pageModel.SettingsMealsUsername; }
            set { _pageModel.SettingsMealsUsername = value; OnPropertyChanged(); }
        }

        public string _selectedCategory { get; set; }
        public string SelectedCategory
        {
            get { return _selectedCategory; }
            set { _selectedCategory = value; OnPropertyChanged(); }
        }
        public DateTime SelectedDate { get; set; }

        private string _selectedRecipe;
        public string SelectedRecipe
        {
            get { return _selectedRecipe; }
            set { _selectedRecipe = value; OnPropertyChanged(); }
        }

        private List<string> _recipeNames;

        public List<string> RecipeNames
        {
            get { return _recipeNames; }
            set { _recipeNames = value; OnPropertyChanged(); }
        }

        private void LoadRecipeNames()
        {
            RecipeNames = DataSharing.Instance.Context.Recipes.Select(i => i.recipe_name).ToList();
        }


        //adaugare reteta

        private List<string> _ingredientNames;

        public List<string> IngredientNames
        {
            get { return _ingredientNames; }
            set { _ingredientNames = value; OnPropertyChanged(); }
        }

        private void LoadIngredientNames()
        {
            // Retrieve ingredient names from the database
            IngredientNames = DataSharing.Instance.Context.Ingredients.Select(i => i.ingredient_name).ToList();
        }

        public string SettingsRetetaNumeValue
        {
            get { return _pageModel.SettingsRetetaNume; }
            set { _pageModel.SettingsRetetaNume = value; OnPropertyChanged(); }
        }

        public string SettingsRetetaDescriereValue
        {
            get { return _pageModel.SettingsRetetaDescriere; }
            set { _pageModel.SettingsRetetaDescriere = value; OnPropertyChanged(); }
        }
        public int SettingsRetetaTimpValue
        {
            get { return _pageModel.SettingsRetetaTimp; }
            set { _pageModel.SettingsRetetaTimp = value; OnPropertyChanged(); }
        }
        public string SettingsRetetaInstructiuniValue
        {
            get { return _pageModel.SettingsRetetaInstructiuni; }
            set { _pageModel.SettingsRetetaInstructiuni = value; OnPropertyChanged(); }
        }
        public int SettingsRetetaCaloriiValue
        {
            get { return _pageModel.SettingsRetetaCalorii; }
            set { _pageModel.SettingsRetetaCalorii = value; OnPropertyChanged(); }
        }
        public int SettingsRetetaProteineValue
        {
            get { return _pageModel.SettingsRetetaProteine; }
            set { _pageModel.SettingsRetetaProteine = value; OnPropertyChanged(); }
        }
        public int SettingsRetetaCarbohidratiValue
        {
            get { return _pageModel.SettingsRetetaCarbohidrati; }
            set { _pageModel.SettingsRetetaCarbohidrati = value; OnPropertyChanged(); }
        }

        public string _selectedIn1 { get; set; }
        public string _selectedIn2 { get; set; }
        public string _selectedIn3 { get; set; }
        public string SelectedIn1
        {
            get { return _selectedIn1; }
            set
            {
                _selectedIn1 = value;
                OnPropertyChanged();
            }
        }

        public string SelectedIn2
        {
            get { return _selectedIn2; }
            set
            {
                _selectedIn2 = value;
                OnPropertyChanged();
            }
        }

        public string SelectedIn3
        {
            get { return _selectedIn3; }
            set
            {
                _selectedIn3 = value;
                OnPropertyChanged();
            }
        }

        private void UpdateIngredientNames()
        {
            // Create a new list to avoid modifying the list being iterated
            List<string> updatedIngredientNames = new List<string>();

            if (!string.IsNullOrEmpty(SelectedIn1))
                updatedIngredientNames.Add(SelectedIn1);

            if (!string.IsNullOrEmpty(SelectedIn2))
                updatedIngredientNames.Add(SelectedIn2);

            if (!string.IsNullOrEmpty(SelectedIn3))
                updatedIngredientNames.Add(SelectedIn3);

            // Remove duplicates from the list if needed
            updatedIngredientNames = updatedIngredientNames.Distinct().ToList();

            // Assign the new list to IngredientNames
            IngredientNames = updatedIngredientNames;
        }


        //butoane

        //save ingredient
        public ICommand SaveCommandIngrediente { get; private set; }
        private void SaveCommandContExecute(object parameter)
        {
            try
            {
                // Check if the new username already exists
                var existingUser = DataSharing.Instance.Context.Users.FirstOrDefault(u => u.username == SettingsContUsernameNouValue);

                if (existingUser == null)
                {
                    // Create a new user object
                    var newUser = new User
                    {
                        username = SettingsContUsernameNouValue,
                        password = SettingsContParolaNouaValue,
                        email = SettingsContEmailNouValue,
                        role = SettingsContValue ? "admin" : "user"
                    };

                    // Add the new user to the database
                    DataSharing.Instance.Context.Users.InsertOnSubmit(newUser);
                    DataSharing.Instance.Context.SubmitChanges();

                    MessageBox.Show("Contul a fost adăugat cu succes.");
                }
                else
                {
                    MessageBox.Show("Username-ul introdus există deja. Alegeți alt username.");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                MessageBox.Show($"A apărut o eroare: {ex.Message}");
            }
        }

        //save cont
        public ICommand SaveCommandCont { get; private set; }
        public void SaveCommandIngredienteExecute(object parameter)
        {
            try
            {

                // Create a new Ingredient object
                var newIngredient = new Ingredient
                {
                    ingredient_name = SettingsIngredienteNumeValue,
                    category = SettingsIngredienteCategorieValue,
                    price = SettingsIngredientePretValue
                    // Add other properties as needed
                };

                // Add the new ingredient to the database
                DataSharing.Instance.Context.Ingredients.InsertOnSubmit(newIngredient);
                DataSharing.Instance.Context.SubmitChanges();

                LoadIngredientNames();

                MessageBox.Show("Ingredientul a fost adăugat cu succes.");
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                MessageBox.Show($"A apărut o eroare: {ex.Message}");
            }
        }

        //save meals
        public ICommand SaveCommandMeals { get; private set; }
        private void SaveCommandMealsExecute(object parameter)
        {
            try
            {
                // Retrieve user ID based on the username
                var userId = DataSharing.Instance.Context.Users
                                .Where(u => u.username == SettingsMealsUsernameValue)
                                .Select(u => u.user_id)
                                .FirstOrDefault();

                if (userId == 0)
                {
                    MessageBox.Show("User not found.");
                    return;
                }

                // Create a new Meals object
                var newMeal = new Meal
                {
                    user_id = userId,
                    meal_type = SelectedCategory,
                    date = SelectedDate
                    // Add other properties as needed
                };

                // Add the new meal to the Meals table
                DataSharing.Instance.Context.Meals.InsertOnSubmit(newMeal);
                DataSharing.Instance.Context.SubmitChanges();

                // Retrieve the ID of the newly added meal
                var mealId = newMeal.meal_id;

                // Link the meal to the selected recipe in Meals_recipes and Recipes tables
                var recipeId = DataSharing.Instance.Context.Recipes
                                    .Where(r => r.recipe_name == SelectedRecipe)
                                    .Select(r => r.recipe_id)
                                    .FirstOrDefault();

                if (recipeId != 0)
                {
                    // Create a new Meals_recipes object
                    var newMealsRecipe = new Meals_recipe
                    {
                        meal_id = mealId,
                        recipe_id = recipeId
                    };

                    // Add the new relationship to the Meals_recipes table
                    DataSharing.Instance.Context.Meals_recipes.InsertOnSubmit(newMealsRecipe);
                }

                // Submit changes to save the new relationships
                DataSharing.Instance.Context.SubmitChanges();

                MessageBox.Show("Meal created successfully.");
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private ObservableCollection<string> _mealCategories;
        public ObservableCollection<string> MealCategories
        {
            get { return _mealCategories; }
            set
            {
                _mealCategories = value;
                OnPropertyChanged();
            }
        }

        //save reteta
        public ICommand SaveCommandReteta { get; private set; }

        private void SaveCommandRetetaExecute(object parameter)
        {
            try
            { 

                UpdateIngredientNames();
                // Save data to the Recipes table
                Recipe newRecipe = new Recipe
                {
                    recipe_name = SettingsRetetaNumeValue,
                    description = SettingsRetetaDescriereValue,
                    preparation_time = SettingsRetetaTimpValue,
                    instructions = SettingsRetetaInstructiuniValue
                };

                DataSharing.Instance.Context.Recipes.InsertOnSubmit(newRecipe);
                DataSharing.Instance.Context.SubmitChanges();

                // Save data to the Nutritional_Informations table
                Nutrition_Information newNutritionalInfo = new Nutrition_Information
                {
                    calories = SettingsRetetaCaloriiValue,
                    protein = SettingsRetetaProteineValue,
                    carbohydrates = SettingsRetetaCarbohidratiValue,
                    recipe_id = newRecipe.recipe_id // Assuming Id is the primary key of the Recipe table
                };

                DataSharing.Instance.Context.Nutrition_Informations.InsertOnSubmit(newNutritionalInfo);
                DataSharing.Instance.Context.SubmitChanges();

                // Save selected ingredients to the Recipes_Ingredients table
                SaveIngredients(newRecipe.recipe_id, SelectedIn1, 1);
                SaveIngredients(newRecipe.recipe_id, SelectedIn2, 1);
                SaveIngredients(newRecipe.recipe_id, SelectedIn3, 1);

                MessageBox.Show("Reteta a fost adaugata cu succes.");
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                MessageBox.Show($"A aparut o eroare: {ex.Message}");
            }
        }

        private void SaveIngredients(int recipeId, string ingredientName, int quantity)
        {
            if (!string.IsNullOrEmpty(ingredientName))
            {
                // Find the ingredient by name
                Ingredient ingredient = DataSharing.Instance.Context.Ingredients
                    .FirstOrDefault(i => i.ingredient_name == ingredientName);

                if (ingredient != null)
                {
                    // Save the association in the Recipes_Ingredients table
                    Recipes_Ingredient recipeIngredient = new Recipes_Ingredient
                    {
                        recipe_id = recipeId,
                        ingredient_id = ingredient.ingredient_id,
                        quantity = quantity
                    };

                    DataSharing.Instance.Context.Recipes_Ingredients.InsertOnSubmit(recipeIngredient);
                    DataSharing.Instance.Context.SubmitChanges();
                }
            }
        }




        public SettingVM()
        {
            _pageModel = new PageModel();

            LoadRecipeNames();
            LoadIngredientNames();

            MealCategories = new ObservableCollection<string>
        {
            "breakfast",
            "lunch",
            "dinner",
        };

            SaveCommandCont = new RelayCommand(SaveCommandContExecute);
            SaveCommandIngrediente = new RelayCommand(SaveCommandIngredienteExecute);
            SaveCommandMeals = new RelayCommand(SaveCommandMealsExecute);
            SaveCommandReteta = new RelayCommand(SaveCommandRetetaExecute);

        }


    }




}
