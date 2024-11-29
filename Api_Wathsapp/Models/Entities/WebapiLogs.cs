using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Api_Wathsapp.Models
{
    public class WebapiLogs
    {
        public int Id { get; set; }
        public int Usu { get; set; }

        [StringLength(50)]
        public string Proceso { get; set; }

        [StringLength(8000)]
        public string Texto { get; set; }

        [StringLength(50)]
        public string Clave { get; set; }

        public DateTime FechaLog { get; set; }
    }
}
