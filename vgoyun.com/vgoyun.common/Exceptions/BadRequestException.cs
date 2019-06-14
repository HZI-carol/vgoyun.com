using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vgoyun.common.Exceptions
{
    public class BadRequestException : Exception
    {
        public int ErrorCode { get; set; }

        public BadRequestException(int code)
            : this(code, "")
        {

        }

        public BadRequestException(int code, string message)
            : base(message)
        {
            this.ErrorCode = code;
        }
    }
}
