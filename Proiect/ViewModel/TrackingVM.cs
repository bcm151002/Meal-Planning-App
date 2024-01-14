using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tema.Model;
using Tema.Utilities;
namespace Tema.ViewModel
{
    class TrackingVM : Utilities.ViewModelBase
    {
        public TrackingVM()
        {
            _pageModel = new PageModel();
            LoadDataFromDatabase();

        }



        private readonly PageModel _pageModel;

        public decimal KCAmount
        {
            get { return _pageModel.KCAmountValue; }
            set { _pageModel.KCAmountValue = value; OnPropertyChanged(); }
        }

        public decimal ProteinAmount
        {
            get { return _pageModel.ProteinAmountValue; }
            set { _pageModel.ProteinAmountValue = value; OnPropertyChanged(); }
        }

        public decimal CarhobydratesAmount
        {
            get { return _pageModel.CarbohydratesAmountValue; }
            set { _pageModel.CarbohydratesAmountValue = value; OnPropertyChanged(); }
        }

        public decimal RecypesAmount
        {
            get { return _pageModel.RecypesAmountValue; }
            set { _pageModel.RecypesAmountValue = value; OnPropertyChanged(); }
        }

        private void LoadDataFromDatabase()
        {
                var context = DataSharing.Instance.Context;

                var username = DataSharing.Instance.Username;
            try
            {
                var userId = context.Users
                    .Where(u => u.username == username)
                    .Select(u => u.user_id)
                    .FirstOrDefault();

                if (userId != 0)
                {
                    var favoriteRecipeIds = context.Users_recipes
                        .Where(ur => ur.user_id == userId && ur.favorite)
                        .Select(ur => ur.recipe_id)
                        .ToList();

                    var nutritionInfo = context.Nutrition_Informations
                        .Where(ni => favoriteRecipeIds.Contains(ni.recipe_id))
                        .GroupBy(ni => ni.recipe_id)
                        .Select(group => new
                        {
                            RecipeId = group.Key,
                            TotalCalories = group.Sum(ni => ni.calories),
                            TotalProteins = group.Sum(ni => ni.protein),
                            TotalCarbohydrates = group.Sum(ni => ni.carbohydrates)
                        })
                        .ToList();

                    var totalFavoriteRecipes = favoriteRecipeIds.Count;

                    var totalCalories = nutritionInfo.Sum(info => info.TotalCalories);

                    var totalProteins = nutritionInfo.Sum(info => info.TotalProteins);

                    var totalCarbohydrates = nutritionInfo.Sum(info => info.TotalCarbohydrates);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        KCAmount = totalCalories;
                        ProteinAmount = totalProteins;
                        CarhobydratesAmount = totalCarbohydrates;
                        RecypesAmount = totalFavoriteRecipes;
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"A apărut o eroare la încărcarea datelor: {ex.Message}");
            }
            
        }

    }
}