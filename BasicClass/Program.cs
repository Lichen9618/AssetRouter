using System;
using DirectedGraph;
using RPCQuery;
using RPCQuery.RPCHelper;
using Config;
using Newtonsoft.Json;
using SystemLink = System.Collections.Generic;


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
            DirectedGraph<string, int> graph = new DirectedGraph<string, int>();
            foreach (var pair in SwapPairs) 
            {
                graph.AddEdge(pair.StartAsset.AssetName, pair.EndAsset.AssetName, 1);
            }
            Console.WriteLine("Start Asset: ");
            string startAsset = Console.ReadLine();
            Console.WriteLine("End Asset: ");
            string endAsset = Console.ReadLine();
            LinkedList<LinkedList<Node<string, int>>> results = graph.Search(startAsset, endAsset);
            foreach (LinkedList<Node<string, int>> path in results) 
            {
                //对每一条path进行一条rpc查询
                SystemLink.List<TypeNValue> AssetPath = new SystemLink.List<TypeNValue>();
                foreach(Node<string, int> Asset in path) 
                {
                    AssetPath.Add(new TypeNValue()
                    {
                        type = "Hash160",
                        value = assetList.Find( T => T.AssetName.Equals(Asset.Value)).AssetHash
                    });
                }
                TypeNValue objs = new TypeNValue()
                {
                    type = "Array",
                    value = AssetPath.ToArray()
                };
                var Lists = new SystemLink.List<TypeNValue>()
                {
                    new TypeNValue()
                    {
                        type = "Integer",
                        value = 100000000
                    },
                    objs
                };
                object[] parameters = new object[]
                {
                    "5ea2866235ab389fdd44017059eac95ca9e247aa",
                    "getAmountsOut",
                    Lists
                };
                QueryParams queryParams = new QueryParams()
                {
                    jsonrpc = "2.0",
                    method = "invokefunction",
                    @params = parameters,
                    id = 3
                };
                string queryJson = JsonConvert.SerializeObject(queryParams);
                string rawQueryResult = SwapCheck.SwapQuery(queryJson);
                var queryResult = JsonConvert.DeserializeObject<ResponseParams>(rawQueryResult);
                TypeNValue[] typeNValues = queryResult.result.stack;
                Console.WriteLine("Result: " + typeNValues[typeNValues.Length - 1]);
            }
            Console.ReadLine();
        }
    }
}
