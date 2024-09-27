using MicroServicioRecetas.Exceptions;
using System;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Filters;
namespace MicroServicioRecetas.Presentation.Filters
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            int statusCode;
            string reasonPhrase;
            var ex = context.Exception;

            switch (ex)
            {
                case UnauthorizedAccessException _:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    reasonPhrase = "Acceso no autorizado";
                    break;
                case InvalidOperationException _:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    reasonPhrase = "Operación inválida";
                    break;
                case NotFoundException _:
                    statusCode = (int)HttpStatusCode.NotFound;
                    reasonPhrase = "Elemento no encontrado";
                    break;
                case SqlException _:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    reasonPhrase = "Error en la base de datos";
                    break;
                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    reasonPhrase = "Error en el servidor";
                    break;
            }

            context.Response = new HttpResponseMessage((HttpStatusCode)statusCode)
            {
                Content = new StringContent(ExceptionMessage(ex)),
                ReasonPhrase = reasonPhrase
            };
        }
        private string ExceptionMessage(System.Exception ex)
        {
            var sb = new StringBuilder();
            var innerEx = ex.InnerException;
            while (innerEx != null)
            {
                sb.Append(innerEx.Message);
                innerEx = innerEx.InnerException;
            }

            return sb.Length > 0 ? sb.ToString() : ex.Message;
        }
        private string ExceptionMessageStackTrace(System.Exception ex)
        {
            var sb = new StringBuilder();
            var innerEx = ex.InnerException;
            while (innerEx != null)
            {
                sb.AppendLine(innerEx.Message);
                innerEx = innerEx.InnerException;
            }
            sb.AppendLine(ex.Message);
            sb.AppendLine(ex.StackTrace);
            return sb.ToString();
        }
        private string ExceptionStackTrace(System.Exception ex)
        {
            var sb = new StringBuilder();
            var innerEx = ex.InnerException;
            while (innerEx != null)
            {
                sb.AppendLine(innerEx.StackTrace);
                innerEx = innerEx.InnerException;
            }
            sb.AppendLine(ex.StackTrace);
            return sb.ToString();
        }
    }
}