using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema.Utilities
{
    internal class IngredientsHistory
    {
        private static IngredientsHistory instance;
        private List<Ingredients> ingredientsList;
        public static IngredientsHistory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new IngredientsHistory();
                }
                return instance;
            }
        }

        private IngredientsHistory()
        {
            ingredientsList = new List<Ingredients>();
        }

        public List<Ingredients> GetIngredients()
        {
            return ingredientsList;
        }
        public void Add(int ingId, string name, string category, decimal price, decimal quantity)
        {
            Ingredients existingIngredient = ingredientsList.Find(ing => ing.ingId == ingId);

            if (existingIngredient == null)
            {
                Ingredients newIngredient = new Ingredients(ingId, name, category, price, quantity);
                ingredientsList.Add(newIngredient);
            }
            else
            {
                existingIngredient.AddQuantity(quantity);
            }
        }
        public decimal GetTotalPrice()
        {
            decimal totalPrice = 0;

            foreach (var ingredient in ingredientsList)
            {
                totalPrice += ingredient.quantity * ingredient.price;
            }

            return totalPrice;
        }

    }
    public class Ingredients
    {
        public int ingId { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public decimal price { get; set; }
        public decimal quantity { get; set; }

        public Ingredients(int IngId, string Name, string Category, decimal Price, decimal Quantity)
        {
            ingId = IngId;
            name = Name;
            category = Category;
            price = Price;
            quantity = Quantity;
        }

        public void AddQuantity(decimal quantToAdd)
        {
            quantity += quantToAdd;
        }
    }
}
