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
        private PropertyInfo[] _array;

        public ListGetter(IGenericRepository<BaseObject> repo, BaseObject model)
        {
            _repo = repo.GetAll();
            _model = model;
            _array = model.GetType().GetProperties();
        }

        public IEnumerable<BaseObject> AddElements()
        {
            // add security for ID member
            
            _array = _model.GetType().GetProperties();
            _result = _repo.Where(o => o.GetType().GetProperty(_array[0].Name).GetValue(o, null) == _model.GetType().GetProperty(_array[0].Name).GetValue(_model, null));
            foreach (var p in _array)
            {
                if (Type.GetTypeCode(p.PropertyType) != TypeCode.String &&
                    Type.GetTypeCode(p.PropertyType) != TypeCode.Decimal &&
                    Type.GetTypeCode(p.PropertyType) != TypeCode.Int32)
                {
                    _result =
                        _result.Where(
                            o =>
                            {
                                var a = o.GetType().GetProperty(p.Name).GetValue(o, null);
                                var b = _model.GetType().GetProperty(p.Name).GetValue(_model, null);
                                var res = true;
                                res = b.GetType().GetProperty("Name").GetValue(b, null) != null &&
                                !b.GetType().GetProperty("Name").GetValue(b, null).Equals("")
                                    ? a.GetType().GetProperty("Name").GetValue(a, null) ==
                                      b.GetType().GetProperty("Name").GetValue(b, null)
                                    :
                                true;
                                return res;
                            }
                            );
                }
                else
                {
                    _result =
                       _result.Where(
                           o => _model.GetType().GetProperty(p.Name).GetValue(_model, null) != null &&
                                !_model.GetType().GetProperty(p.Name).GetValue(_model, null).Equals("") ?
                                o.GetType().GetProperty(p.Name).GetValue(o, null) ==
                                _model.GetType().GetProperty(p.Name).GetValue(_model, null) : true);
                }


            }
            return _result;
        }
    }
}
