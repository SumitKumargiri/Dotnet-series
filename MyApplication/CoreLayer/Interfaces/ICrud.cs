using CoreLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Interfaces
{
    public interface ICrud
    {
        Task<ResultModel<object>> GetDataAsync();
    }

}
