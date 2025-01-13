using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IWhatsAppService
    {
        Task<List<MensajesWhatsapp>> GetAll();
        Task<List<MensajesWhatsapp>> GetByTelefono(string Telefono);
        Task<dynamic> EnviarMensaje(Mensaje entry);
        Task<dynamic> InciarCharla(HelloWorld entry);
        string Webhook(string mode,string challenge,string verify_token);
        dynamic RecibirMensaje(WebHookResponseModel entry);
    }
}
