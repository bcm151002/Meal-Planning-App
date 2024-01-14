using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Tema.Model
{
    public class PageModel
    {
        //Pagina customers
        public int CustomerCount { get; set; }
        public string CustomerUsername { get; set; }
        public string CustomerUsernameNou { get; set; }
        public string CustomerParolaNoua { get; set; }
        public string CustomerEmailNou { get; set; }
        public string CustomerAccType { get; set; }

        //public ICommand SaveCommand;

        //Pagina home

        //pagina da
        public string ProductStatus { get; set; }
        public string OrderDate { get; set; }

        //pagina de tracking nutritional information
        public decimal KCAmountValue { get; set; }
        public decimal ProteinAmountValue {  get; set; }
        public decimal CarbohydratesAmountValue { get; set; }
        public decimal RecypesAmountValue { get; set; }

            //pagina de settings
        //Cont
        public string SettingsContUsernameNou { get; set; }
        public string SettingsContParolaNoua { get; set; }
        public string SettingsContEmailNou { get; set; }
        public bool SettingsContRole {  set; get; }
        //ingredient
        public string SettingsIngredienteNume {  get; set; }
        public string SettingsIngredienteCategorie {  get; set; }
        public int SettingsIngredientePret {  get; set; }
        //meals
        public string SettingsMealsUsername { get; set; }
        //reteta
        public string SettingsRetetaNume {  get; set; }
        public string SettingsRetetaDescriere { get; set; }
        public int SettingsRetetaTimp { get; set; }
        public string SettingsRetetaInstructiuni { get; set; }
        public int SettingsRetetaCalorii { get; set; }
        public int SettingsRetetaProteine { get; set; }
        public int SettingsRetetaCarbohidrati { get; set; }





        public string ShipmentDelivery { get; set; }

        //aici pun eu ce functii am nevoie si ce trebuie sa apelez in
        //paginile de la vm
    }
}