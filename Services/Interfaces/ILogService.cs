using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ILoginService
    {
        void Log(int Usu, string proceso, string texto, string indicador);
    }
}
