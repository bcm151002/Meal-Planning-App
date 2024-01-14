using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tema.Database;

namespace Tema.Utilities
{
    internal class DataSharing
    {
        // Singleton
        private static DataSharing _instance;
        private static readonly object LockObject = new object();

        // Variabila pentru stocarea username-ului
        private string _username;
        private dbDataContext _context;

        // Proprietate pentru accesarea username-ului
        public string Username
        {
            get { return _username; }
            set
            {
                // Aici puteți adăuga orice logica suplimentară necesară înainte de a seta valoarea
                _username = value;
            }
        }

        public dbDataContext Context
        {
            get { return _context; }
            set
            { _context = value; }
        }

        // Constructor privat pentru a preveni crearea de instanțe externe
        private DataSharing()
        {
            // Inițializați orice alte resurse necesare aici
        }

        // Metoda pentru a obține instanța singleton
        public static DataSharing Instance
        {
            get
            {
                lock (LockObject)
                {
                    return _instance ?? (_instance = new DataSharing());
                }
            }
        }

        // Alte metode și funcționalități pot fi adăugate după necesitate
    }
}

