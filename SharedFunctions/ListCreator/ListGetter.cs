using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using SharedFunctions.LambdaGenerator;
using ShoppingCart.Models.Models.Entities;
using ShoppingCart.Models.Repositories.Interface;

namespace ShoppingCart.GeneralLib.ListCreator
{
    public class ListGetter
    {
        private IList<BaseObject> _repo;
        private IEnumerable<BaseObject> _result;
        private BaseObject _model; 

        public ListGetter(IGenericRepository<BaseObject> repo, BaseObject model)
        {
            _repo = repo.GetAll();
            _model = model;
        }

        public IEnumerable<BaseObject> AddElements()
        {
            
            PropertyInfo[]  array = _model.GetType().GetProperties();
            _result = _repo.Where(o => o.GetType().GetProperty(array[0].Name).GetValue(o, null) == _model.GetType().GetProperty(array[0].Name).GetValue(_model, null));
            foreach (var p in array)
            {
                _result =
                    _result.Where(
                        o => _model.GetType().GetProperty(p.Name).GetValue(p, null) != null &&
                            ! _model.GetType().GetProperty(p.Name).GetValue(p, null).Equals("") &&
                            o.GetType().GetProperty(p.Name).GetValue(o, null) ==
                            _model.GetType().GetProperty(p.Name).GetValue(p, null));
            }
            return _result;
        }
    }
}
