using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vgoyun.common.Jwt
{
    public class JweHeader
    {
        /// <summary>
        /// (Algorithm) Header Parameter
        /// </summary>
        public string alg { get; set; }

        /// <summary>
        /// (Encryption Algorithm) Header Parameter
        /// </summary>
        public string enc { get; set; }
    }
}
