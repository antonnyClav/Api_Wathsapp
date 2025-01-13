using Data.Context;
using System;
using System.Linq;

namespace Data.Entities
{
    public class Mensajes
    {
        private readonly ApplicationDbContext context;
        public Mensajes(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void InsertarMensaje(bool recibido,string mensaje_recibido, string id_wa, string telefono_wa)
        {
            //PARAMETRO
            Parametros parametro = context.Parametros.Where(x => x.Codigo == "telefonoWhatsapp1" && x.BajaFecha == null).FirstOrDefault();
            string telefonoWhatsapp = parametro.Valor;
            parametro = null;

            DateTime hoy = DateTime.Now;
            MensajesWhatsapp mensaje = new MensajesWhatsapp();
            mensaje.Remitente = recibido ? telefono_wa: telefonoWhatsapp;
            mensaje.Destinatario = !recibido ? telefono_wa : telefonoWhatsapp;
            mensaje.IdWa = id_wa;
            mensaje.Mensaje = mensaje_recibido;
            mensaje.Adjunto = "";
            mensaje.Origen = "";
            mensaje.Accion = "";
            mensaje.IdRef = 0;
            mensaje.RefEntidad = "";
            mensaje.Enviado = !recibido;
            mensaje.Recibido = recibido;
            mensaje.Leido = false;
            mensaje.Obs = "";
            mensaje.AltaFecha = hoy;
            mensaje.UsuId = 1;
            mensaje.Filler = "";
            context.Add(mensaje);
            context.SaveChanges();
        }
    }
    public class WebHookResponseModel
    {
        public Entry[] entry { get; set; }
    }
    public class Entry
    {
        public Change[] changes { get; set; }
    }
    public class Change
    {
        public Value value { get; set; }
    }
    public class Value
    {
        public int ad_id { get; set; }
        public long form_id { get; set; }
        public long leadgen_id { get; set; }
        public int created_time { get; set; }
        public long page_id { get; set; }
        public int adgroup_id { get; set; }
        public Messages[] messages { get; set; }
    }
    public class Messages
    {
        public string id { get; set; }
        public string from { get; set; }
        public Text2 text { get; set; }
    }
    public class Text2
    {
        public string body { get; set; }
    }
}
