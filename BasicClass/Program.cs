using System;
using DirectedGraph;
using RPCQuery;
using RPCQuery.RPCHelper;
using Config;
using Newtonsoft.Json;

namespace BasicClass
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigReader config = new ConfigReader();
            var assetList = config.GetAllAsset();
            var CallContract = config.GetCallContract();
            var SwapPairs = config.GetAllSwapPair();
            #region Json构造
            //TypeNValue obj1 = new TypeNValue()
            //{
            //    type = "Hash160",
            //    value = "f46719e2d16bf50cddcef9d4bbfece901f73cbb6"
            //};
            //TypeNValue obj2 = new TypeNValue()
            //{
            //    type = "Hash160",
            //    value = "282e3340d5a1cd6a461d5f558d91bc1dbc02a07b"
            //};
            //TypeNValue obj3 = new TypeNValue()
            //{
            //    type = "Hash160",
            //    value = "534dcac35b0dfadc7b2d716a7a73a7067c148b37"
            //};
            //TypeNValue obj4 = new TypeNValue()
            //{
            //    type = "Hash160",
            //    value = "f46719e2d16bf50cddcef9d4bbfece901f73cbb6"
            //};
            //TypeNValue objs = new TypeNValue()
            //{
            //    type = "Array",
            //    value = new TypeNValue[] { obj1,obj2,obj3,obj4 }
            //};
            //var Lists = new System.Collections.Generic.List<TypeNValue>()
            //{
            //    new TypeNValue()
            //    {
            //        type = "Integer",
            //        value = 100000000
            //    },
            //    objs
            //};
            //object[] parameters = new object[]
            //{
            //    "5ea2866235ab389fdd44017059eac95ca9e247aa",
            //    "getAmountsOut",
            //    Lists
            //};

            //QueryParams queryParams = new QueryParams()
            //{
            //    jsonrpc = "2.0",
            //    method = "invokefunction",
            //    @params = parameters,
            //    id = 3
            //};

            //string queryJson = JsonConvert.SerializeObject(queryParams);
            //Console.WriteLine(queryJson);
            //string queryResult = SwapCheck.SwapQuery(queryJson);

            //var test = JsonConvert.DeserializeObject<ResponseParams>(queryResult);
            #endregion

            #region Graph绘制
            //DirectedGraph<string, int> graph = new DirectedGraph<string, int>();
            //graph.AddEdge("FLM", "NEO", 1);
            //graph.AddEdge("NEO", "BTC", 1);

            //graph.AddEdge("NEO", "USDT", 1);

            //graph.AddEdge("FLM", "USDT", 1);
            //graph.AddEdge("USDT", "BTC", 2);
            //graph.AddEdge("NEO", "ETH", 1);
            //LinkedList<LinkedList<Node<string, int>>> results = graph.Search("NEO", "BTC");
            //printResults("A-B (Depth <= 1)", results);
            #endregion
            Console.ReadLine();
        }
    }
}
