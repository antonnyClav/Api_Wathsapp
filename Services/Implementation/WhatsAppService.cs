using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class WhatsAppService : IWhatsAppService
    {        
        private readonly ApplicationDbContext _context;

        public WhatsAppService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MensajesWhatsapp>> GetAll()
        {
            try
            {
                var reply = await _context.MensajesWhatsapps.ToListAsync();
                return reply;
            }
            catch (Exception ex)
            {
                var reply = new List<MensajesWhatsapp> { new MensajesWhatsapp { Mensaje = ex.Message } };
                return reply;
            }
       }

        public async Task<List<MensajesWhatsapp>> GetByTelefono(string Telefono)
        {
            try
            {
                var reply = await _context.MensajesWhatsapps.Where(x => x.Remitente.Contains(Microsoft.VisualBasic.Strings.Right(Telefono, 8)) || x.Destinatario.Contains(Microsoft.VisualBasic.Strings.Right(Telefono, 8))).OrderBy(x => x.AltaFecha).ToListAsync();
                return reply;
            }
            catch (Exception ex)
            {
                var reply = new List<MensajesWhatsapp> { new MensajesWhatsapp { Mensaje = ex.Message } };
                return reply;
            }
        }

        public async Task<dynamic> EnviarMensaje(Mensaje entry)
        {
            try
            {
                //PARAMETROS          
                Parametros parametro = _context.Parametros.Where(x => x.Codigo == "tokenMeta" && x.BajaFecha == null).FirstOrDefault();
                string tokenMeta = parametro.Valor;

                parametro = _context.Parametros.Where(x => x.Codigo == "urlMeta" && x.BajaFecha == null).FirstOrDefault();
                string urlMeta = parametro.Valor;
                parametro = null;

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, urlMeta);
                request.Headers.Add("Authorization", "Bearer " + tokenMeta);
                var content = new StringContent(JsonConvert.SerializeObject(entry), null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                //return await response.Content.ReadAsStreamAsync();
                var responseOUT = new HttpResponseMessage(response.StatusCode);

                Mensajes dat = new Mensajes(_context);
                //INSERTAMOS LOS DATOS RECIBIDOS
                dat.InsertarMensaje(false, entry.text.body, "", entry.to);

                return responseOUT;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<dynamic> InciarCharla(HelloWorld entry)
        {
            try
            {
                //PARAMETROS          
                Parametros parametro = _context.Parametros.Where(x => x.Codigo == "tokenMeta" && x.BajaFecha == null).FirstOrDefault();
                string tokenMeta = parametro.Valor;

                parametro = _context.Parametros.Where(x => x.Codigo == "urlMeta" && x.BajaFecha == null).FirstOrDefault();
                string urlMeta = parametro.Valor;
                parametro = null;

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, urlMeta);
                request.Headers.Add("Authorization", "Bearer " + tokenMeta);
                var content = new StringContent(JsonConvert.SerializeObject(entry), null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                //return await response.Content.ReadAsStreamAsync();
                var responseOUT = new HttpResponseMessage(response.StatusCode);
                Mensajes dat = new Mensajes(_context);
                //INSERTAMOS LOS DATOS RECIBIDOS                
                dat.InsertarMensaje(false, entry.template.name, "", entry.to);

                return responseOUT;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string Webhook(string mode, string challenge, string verify_token)
        {
            string tokenApi = "BEW.PPASTAHWGNITURCER";
            Parametros parametro = _context.Parametros.Where(x => x.Codigo == "tokenApiWSOvidio" && x.BajaFecha == null).FirstOrDefault();
            tokenApi = parametro.Valor;
            parametro = null;

            try
            {
                //SI EL TOKEN ES hola (O EL QUE COLOQUEMOS EN FACEBOOK)
                if (verify_token.Equals(tokenApi))
                {
                    return challenge;
                }
                else
                {
                    return "TOKEN INVALIDO";
                }
            }
            catch (Exception)
            {
                return "TOKEN INVALIDO";
            }
        }

        public dynamic RecibirMensaje(WebHookResponseModel entry)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain");

                //SI NO HAY MENSAJES RETORNAMOS UN OK            
                if (entry.entry[0].changes[0].value.messages == null) return response;

                //EXTRAEMOS EL MENSAJE RECIBIDO
                string mensaje_recibido = "";
                if (entry.entry[0].changes[0].value.messages[0].text != null)
                {
                    mensaje_recibido = entry.entry[0].changes[0].value.messages[0].text.body;
                }
                //EXTRAEMOS EL ID UNICO DEL MENSAJE
                string id_wa = entry.entry[0].changes[0].value.messages[0].id;
                //EXTRAEMOS EL NUMERO DE TELEFONO DEL CUAL RECIBIMOS EL MENSAJE
                string telefono_wa = entry.entry[0].changes[0].value.messages[0].from;
                //INICIALIZAMOS LA CONEXION A LA BD
                Mensajes dat = new Mensajes(_context);
                //INSERTAMOS LOS DATOS RECIBIDOS
                dat.InsertarMensaje(true, mensaje_recibido, id_wa, telefono_wa);
            }
            catch (Exception)
            {
                throw;
            }
            
            //SI NO HAY ERROR RETORNAMOS UN OK            
            return response;            
        }
    }
}
