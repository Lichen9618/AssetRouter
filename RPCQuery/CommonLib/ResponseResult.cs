using System;
using System.Collections.Generic;
using System.Text;

namespace RPCQuery
{
    public class ResponseResult
    {
        public string script;
        public string state;
        public string gas_consumed;
        public TypeNValue[] stack;
    }
}
