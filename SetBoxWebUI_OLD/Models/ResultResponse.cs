using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetBoxWebUI.Models
{
    /// <summary>
    /// Respose
    /// </summary>
    /// <typeparam name="T">Tipo do Retorno</typeparam>
    public class Response<T>
    {
        /// <summary>
        /// objeto de retorno
        /// </summary>
        public T Result { get; set; }
        /// <summary>
        /// Mensagem de erro
        /// </summary>
        public string Message { get; set; } = "";
        /// <summary>
        /// Sessão invalida ou expirada
        /// </summary>
        public bool SessionExpired { get; set; } = false;
        /// <summary>
        /// Teve erro?
        /// </summary>
        public bool Status { get; set; } = true;
    }
}
