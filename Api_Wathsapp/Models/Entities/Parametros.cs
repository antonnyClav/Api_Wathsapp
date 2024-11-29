using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api_Wathsapp.Models
{
    public partial class Parametros
    {
        public int Id { get; set; }
        [StringLength(20)]
        public string Codigo { get; set; }
        [StringLength(50)]
        public string Nombre { get; set; }
        [StringLength(4096)]
        public string Valor { get; set; }
        public bool? ReqAuth { get; set; }
        [StringLength(50)]
        public string Ambito { get; set; }
        public DateTime AltaFecha { get; set; }
        public DateTime? ModiFecha { get; set; }
        public DateTime? BajaFecha { get; set; }
        [StringLength(255)]
        public string Filler { get; set; }
        public int UsuId { get; set; }
    }
}
