using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_Wathsapp.Util.Interfaces
{
    public interface ILoginService
    {
        void Log(int Usu, string proceso, string texto, string indicador);
    }
}
