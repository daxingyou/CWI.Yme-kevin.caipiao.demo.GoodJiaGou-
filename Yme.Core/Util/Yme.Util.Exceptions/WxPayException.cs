using System;
using System.Collections.Generic;
using System.Web;

namespace Yme.Util.Exceptions
{
    public class WxPayException : Exception 
    {
        public WxPayException(string msg) : base(msg) 
        {

        }
     }
}
