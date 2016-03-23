using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Models.Repositories.Interface;

namespace ShoppingCart.GeneralLib.ListCreator
{
    class ListGetter
    {
        private IGenericRepository<Object> _repo;

        public ListGetter(IGenericRepository<Object> repo)
        {
            _repo = repo;
        }

        public void AddElements()
        {
            
        }
    }
}
