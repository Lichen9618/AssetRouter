using System;

namespace RPCQuery
{
    public class QueryParams
    {
        public string jsonrpc;
        public string method;
        public object[] @params;
        public int id;
    }
}
