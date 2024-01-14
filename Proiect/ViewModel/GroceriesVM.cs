using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tema.Model;
using Tema.Utilities;

namespace Tema.ViewModel
{
    class GroceriesVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;
        public GroceriesVM()
        {
            _pageModel = new PageModel();
        }


    }
}
