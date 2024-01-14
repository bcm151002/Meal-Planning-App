using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tema.Model;

namespace Tema.ViewModel
{
     class RecipesVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;

        public RecipesVM()
        {
            _pageModel = new PageModel();
        }
    }
}
