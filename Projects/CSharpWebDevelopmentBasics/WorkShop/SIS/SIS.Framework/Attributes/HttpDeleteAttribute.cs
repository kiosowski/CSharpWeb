using SIS.Framework.Attributes.Methods;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.Framework.Attributes
{
    public class HttpDeleteAttribute : HttpMethodAttribute
    {
        public override bool IsValid(string requestMethod)
        {
            if (requestMethod.ToLower() == "delete")
            {
                return true;
            }
            return false;
        }
    }
}
