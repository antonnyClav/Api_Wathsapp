
using Services.Interfaces;
using System;
using System.Linq;
using Data.Entities;
using Data.Context;

namespace Services.Implementation
{
    public class LoginService : ILoginService
    {
        private readonly ApplicationDbContext _context;
        private bool GraboError = false;

        public LoginService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Log(int Usu, string proceso, string texto, string indicador)
        {
            try
            {
                //habilitado para loguear
                if (!LogDB_Enagle()) return;

                WebapiLogs WebapiLogs = new WebapiLogs();
                WebapiLogs.FechaLog = DateTime.Now;
                WebapiLogs.Usu = Usu;
                WebapiLogs.Proceso = proceso;
                WebapiLogs.Texto = texto;
                WebapiLogs.Clave = indicador;
                _context.webapi_logs.Add(WebapiLogs);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                if (!GraboError)
                {
                    GraboError = true;
                    Log(0, "WebApi", "Log: Error " + ex.Message + "....." + ex.InnerException, "E");
                }
            }
        }
        private bool LogDB_Enagle()
        {
            try
            {               
                string valor = GetParametro("WEBAPI_LOG");
                return valor == "1";
            }
            catch (Exception)
            {
                return false;
            }
        }
        public string GetParametro(string Codigo)
        {
            string strValor = "";
            try
            {
                strValor = _context.Parametros.Where(x => x.BajaFecha == null && x.Codigo == Codigo).FirstOrDefault()?.Valor;
            }
            catch
            {
                strValor = "?";
            }

            return strValor;
        }
    }
}
