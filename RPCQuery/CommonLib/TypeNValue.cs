using System;
using System.Collections.Generic;
using System.Text;

namespace RPCQuery
{
    public class TypeNValue
    {
        public string type;
        public object value;

        public override string ToString() 
        {
            return value.ToString();
        }
    }
}
