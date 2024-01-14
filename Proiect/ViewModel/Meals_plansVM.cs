using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tema.Model;

namespace Tema.ViewModel
{
    class Meals_plansVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;
        public Meals_plansVM()
        {
            _pageModel = new PageModel();
        }
    }
}
