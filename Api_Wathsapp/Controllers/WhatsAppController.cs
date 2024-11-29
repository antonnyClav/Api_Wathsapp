using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using Api_Wathsapp.Models;
using Api_Wathsapp.Models.template;
using Newtonsoft.Json;
using Api_Wathsapp.Util.Interfaces;

// ngrok http 2878 --host-header=localhost:2878 --region us  
// ngrok http https://localhost:344
namespace Api_Wathsapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhatsAppController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ILoginService logService;
        private string tokenApi = "BEW.PPASTAHWGNITURCER";

        public WhatsAppController(ApplicationDbContext context, ILoginService _logService)
        {
            this.context = context;
            this.logService = _logService;

            Parametros parametro = context.Parametros.Where(x => x.Codigo == "tokenApiWSOvidio" && x.BajaFecha == null).FirstOrDefault();
            this.tokenApi = parametro.Valor;
            parametro = null;
        }

        // GET api/Whatsapp
        [Route("GetAll")]
        [HttpGet]
        public ActionResult<IEnumerable<MensajesWhatsapp>> GetAll()
        {
            try
            {
                logService.Log(0, "WebApi", "Whatsapp INICIO GetAll()", "I");
                return context.MensajesWhatsapps.Where(x => x.BajaFecha == null).ToList();
            }
            catch (Exception ex)
            {
                logService.Log(0, "WebApi", "Whatsapp ERROR GetAll() " + ex.Message + "....." + ex.InnerException, "E");
                return BadRequest(ModelState);
            }
            finally
            {
                logService.Log(0, "WebApi", "Whatsapp FIN GetAll()", "F");
            }
        }

        // GET api/Whatsapp/1126432987
        [Route("GetByTelefono/{Telefono}")]
        [HttpGet]
        public ActionResult<IEnumerable<MensajesWhatsapp>> GetByTelefono(string Telefono)
        {
            try
            {
                logService.Log(0, "WebApi", "Whatsapp INICIO GetByTelefono(" + Telefono + ")", "I");
                return context.MensajesWhatsapps.Where(x => x.Remitente.Contains(Microsoft.VisualBasic.Strings.Right(Telefono, 8)) || x.Destinatario.Contains(Microsoft.VisualBasic.Strings.Right(Telefono, 8))).OrderBy(x=> x.AltaFecha).ToList();
            }
            catch (Exception ex)
            {
                logService.Log(0, "WebApi", "Whatsapp ERROR GetByTelefono(Telefono) " + ex.Message + "....." + ex.InnerException, "E");
                return BadRequest(ModelState);
            }
            finally
            {
                logService.Log(0, "WebApi", "Whatsapp FIN GetByTelefono(Telefono)", "F");
            }
        }

        [HttpPost]
        [Route("EnviarMensaje")]
        public async Task<dynamic> EnviarMensaje([FromBody] Mensaje entry)
        {
            try
            {
                logService.Log(0, "WebApi", "Whatsapp INI EnviarMensaje() entry: " + System.Text.Json.JsonSerializer.Serialize(entry), "G");
                //PARAMETROS          
                Parametros parametro = context.Parametros.Where(x => x.Codigo == "tokenMeta" && x.BajaFecha == null).FirstOrDefault();
                string tokenMeta = parametro.Valor;

                parametro = context.Parametros.Where(x => x.Codigo == "urlMeta" && x.BajaFecha == null).FirstOrDefault();
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

                Mensajes dat = new Mensajes(context);
                //INSERTAMOS LOS DATOS RECIBIDOS
                dat.InsertarMensaje(false, entry.text.body, "", entry.to);

                return responseOUT;
            }
            catch (Exception ex)
            {
                logService.Log(0, "WebApi", "Whatsapp ERROR EnviarMensaje() " + ex.Message + "....." + ex.InnerException, "E");
                var error = new { code = 444, message = "Error al enviar el mensaje." };
                return BadRequest(error);
            }
        }

        //ENVIO DE TEMPLATE INICIAL
        [HttpPost]
        [Route("InciarCharla")]
        public async Task<dynamic> InciarCharla([FromBody] HelloWorld entry)
        {
            try
            {
                logService.Log(0, "WebApi", "Whatsapp INI InciarCharla() entry: " + System.Text.Json.JsonSerializer.Serialize(entry), "G");

                //PARAMETROS          
                Parametros parametro = context.Parametros.Where(x => x.Codigo == "tokenMeta" && x.BajaFecha == null).FirstOrDefault();
                string tokenMeta = parametro.Valor;

                parametro = context.Parametros.Where(x => x.Codigo == "urlMeta" && x.BajaFecha == null).FirstOrDefault();
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
                Mensajes dat = new Mensajes(context);
                //INSERTAMOS LOS DATOS RECIBIDOS                
                dat.InsertarMensaje(false, entry.template.name, "", entry.to);

                return responseOUT;
            }
            catch (Exception ex)
            {
                logService.Log(0, "WebApi", "Whatsapp ERROR InciarCharla() " + ex.Message + "....." + ex.InnerException, "E");
                var error = new { code = 444, message = "Error al enviar el mensaje." };
                return BadRequest(error);
            }
        }

        //RECIBIMOS LOS DATOS DE VALIDACION VIA GET
        [HttpGet]
        //DENTRO DE LA RUTA webhook
        [Route("webhook")]
        //RECIBIMOS LOS PARAMETROS QUE NOS ENVIA WHATSAPP PARA VALIDAR NUESTRA URL
        public string Webhook(
            [FromQuery(Name = "hub.mode")] string mode,
            [FromQuery(Name = "hub.challenge")] string challenge,
            [FromQuery(Name = "hub.verify_token")] string verify_token
        )
        {
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
            catch (Exception ex)
            {
                logService.Log(0, "WebApi", "Whatsapp ERROR Webhook() " + ex.Message + "....." + ex.InnerException, "E");
                return "TOKEN INVALIDO";
            }            
        }

        //RECIBIMOS LOS DATOS DE VIA POST
        [HttpPost]
        //DENTRO DE LA RUTA webhook
        [Route("webhook")]
        //RECIBIMOS LOS DATOS Y LOS GUARDAMOS EN EL MODELO WebHookResponseModel
        public dynamic RecibirMensaje([FromBody] WebHookResponseModel entry)
        {
            try
            {
                logService.Log(0, "WebApi", "Whatsapp INI RecibirMensaje() entry: " + System.Text.Json.JsonSerializer.Serialize(entry), "G");
                var response = new HttpResponseMessage(HttpStatusCode.OK);
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
                Mensajes dat = new Mensajes(context);
                //INSERTAMOS LOS DATOS RECIBIDOS
                dat.InsertarMensaje(true, mensaje_recibido, id_wa, telefono_wa);

                //SI NO HAY ERROR RETORNAMOS UN OK            
                return response;
            }
            catch (Exception ex)
            {
                logService.Log(0, "WebApi", "Whatsapp ERROR RecibirMensaje() " + ex.Message + "....." + ex.InnerException, "E");
                var error = new { code = 444, message = "Error al recibir el mensaje." };
                return BadRequest(error);
            }

        }
    }
}
