using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Interfaces;
using Data.Entities;
using Data.DTO;

// ngrok http 2878 --host-header=localhost:2878 --region us  
// ngrok http https://localhost:344
namespace Api_Wathsapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhatsAppController: ControllerBase
    {
        private readonly ILoginService _logService;
        private readonly IWhatsAppService _WhatsAppService;
        
        public WhatsAppController(ILoginService logService, IWhatsAppService whatsAppService)
        {
            _logService = logService;
            _WhatsAppService = whatsAppService;
        }

        // GET api/Whatsapp
        [Route("GetAll")]
        [HttpGet]
        public async Task<ActionResult<ResponseDTO<List<MensajesWhatsapp>>>> GetAll()
        {
            var reply = await _WhatsAppService.GetAll();
            if (reply != null)
            {
                return Ok(new ResponseDTO<List<MensajesWhatsapp>> { status = Data.Enum.Status.OK, data = reply });
            }
            return NotFound(new ResponseDTO<List<MensajesWhatsapp>> { status = Data.Enum.Status.NOTFOUND, messages = "No se encontraron datos" });

        }

        // GET api/Whatsapp/1126432987
        [Route("GetByTelefono/{Telefono}")]
        [HttpGet]
        public async Task<ActionResult<ResponseDTO<List<MensajesWhatsapp>>>> GetByTelefono(string Telefono)
        {
            _logService.Log(0, "WebApi", "Whatsapp INI GetByTelefono() Telefono: " + Telefono, "G");
            var reply = await _WhatsAppService.GetByTelefono(Telefono);
            if (reply != null)
            {
                _logService.Log(0, "WebApi", "Whatsapp INI GetByTelefono() reply: " + System.Text.Json.JsonSerializer.Serialize(reply), "I");
                return Ok(new ResponseDTO<List<MensajesWhatsapp>> { status = Data.Enum.Status.OK, data = reply });
            }
            return NotFound(new ResponseDTO<List<MensajesWhatsapp>> { status = Data.Enum.Status.NOTFOUND, messages = "Error al obtener el telefono" }); 
        }

        [HttpPost]
        [Route("EnviarMensaje")]
        public async Task<dynamic> EnviarMensaje([FromBody] Mensaje entry)
        {
            try
            {
                _logService.Log(0, "WebApi", "Whatsapp INI EnviarMensaje() entry: " + System.Text.Json.JsonSerializer.Serialize(entry), "G");
                var reply = await _WhatsAppService.EnviarMensaje(entry);
                if (reply != null)
                {
                    _logService.Log(0, "WebApi", "Whatsapp INI EnviarMensaje() reply: " + System.Text.Json.JsonSerializer.Serialize(reply), "I");
                    return Ok(reply);
                }

                return new BadRequestObjectResult(new ResponseDTO<object> { status = Data.Enum.Status.NOTFOUND, messages = "El envio del mensaje no arrojo resultados." });                
                
            }
            catch (Exception ex)
            {
                _logService.Log(0, "WebApi", "Whatsapp ERROR EnviarMensaje() " + ex.Message + "....." + ex.InnerException, "E");
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
                _logService.Log(0, "WebApi", "Whatsapp INI InciarCharla() entry: " + System.Text.Json.JsonSerializer.Serialize(entry), "G");

                var reply = await _WhatsAppService.InciarCharla(entry);
                if (reply != null)
                {
                    _logService.Log(0, "WebApi", "Whatsapp INI InciarCharla() reply: " + System.Text.Json.JsonSerializer.Serialize(reply), "I");
                    return Ok(reply);
                }

                return new BadRequestObjectResult(new ResponseDTO<object> { status = Data.Enum.Status.NOTFOUND, messages = "La charla inicial no arrojo resultados." });
            }
            catch (Exception ex)
            {
                _logService.Log(0, "WebApi", "Whatsapp ERROR InciarCharla() " + ex.Message + "....." + ex.InnerException, "E");
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
            var reply = _WhatsAppService.Webhook(mode, challenge, verify_token);            
            return reply;                        
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
                _logService.Log(0, "WebApi", "Whatsapp INI RecibirMensaje() entry: " + System.Text.Json.JsonSerializer.Serialize(entry), "G");

                var reply = _WhatsAppService.RecibirMensaje(entry);
                if (reply != null)
                {
                    _logService.Log(0, "WebApi", "Whatsapp INI RecibirMensaje() reply: " + System.Text.Json.JsonSerializer.Serialize(reply), "I");
                    return Ok(reply);
                }

                return new BadRequestObjectResult(new ResponseDTO<object> { status = Data.Enum.Status.NOTFOUND, messages = "La charla inicial no arrojo resultados." });
            }
            catch (Exception ex)
            {
                _logService.Log(0, "WebApi", "Whatsapp ERROR RecibirMensaje() " + ex.Message + "....." + ex.InnerException, "E");
                var error = new { code = 444, message = "Error al recibir el mensaje." };
                return BadRequest(error);
            }

        }
    }
}
