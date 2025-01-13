using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public partial class MensajesWhatsapp
    {
        public int Id { get; set; }
        [StringLength(255)]
        public string Remitente { get; set; }
        [StringLength(255)]
        public string Destinatario { get; set; }
        [StringLength(255)]
        public string IdWa { get; set; }
        [StringLength(8000)]
        public string Mensaje { get; set; }
        [StringLength(1024)]
        public string Adjunto { get; set; }
        [StringLength(100)]
        public string Origen { get; set; }
        [StringLength(100)]
        public string Accion { get; set; }
        public int IdRef { get; set; }
        [StringLength(100)]
        public string RefEntidad { get; set; }
        public bool Enviado { get; set; }
        public bool Recibido { get; set; }
        public bool Leido { get; set; }
        [StringLength(255)]
        public string Obs { get; set; }
        public DateTime AltaFecha { get; set; }
        public DateTime? ModiFecha { get; set; }
        public DateTime? BajaFecha { get; set; }
        public int UsuId { get; set; }
        [StringLength(255)]
        public string Filler { get; set; }
    }
}
