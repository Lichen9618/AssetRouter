using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Config;
using DirectedGraph;
using RPCQuery;
using RPCQuery.RPCHelper;
using Newtonsoft.Json;
using Lib;

namespace WebAPIService
{
    public class AssetQuery
    {
        public string[] swapPath { get; set; }
        public long[] amount { get; set; }

        public List<AssetQuery> CheckSwapByFlamingo(string StartAsset, string EndAsset, int amount) 
        {
            List<AssetQuery> FinalQueryResult = new List<AssetQuery>();
            ConfigReader config = new ConfigReader();
            List<Asset> assetList = config.GetAllAsset();
            Contract callContract = config.GetCallContract();
            List<SwapPair> swapPairs = config.GetAllSwapPair();
            List<string> allNodes = config.GetAllNodeUrl();
            Graph<string, int> graph = new Graph<string, int>();
            foreach (SwapPair pair in swapPairs) 
            {
                graph.AddEdge(pair.StartAsset.AssetName, pair.EndAsset.AssetName, 1);                
            }
            DirectedGraph.LinkedList<DirectedGraph.LinkedList<Node<string, int>>> results = graph.Search(StartAsset, EndAsset);
            foreach (DirectedGraph.LinkedList<Node<string, int>> path in results) 
            {
                List<TypeNValue> AssetPath = new List<TypeNValue>();
                List<string> onePath = new List<string>();
                foreach (Node<string, int> node in path) 
                {
                    Asset FindedAsset = assetList.Find(T => T.AssetName.Equals(node.Value));
                    onePath.Add(FindedAsset.AssetName);
                    AssetPath.Add(new TypeNValue()
                    {
                        type = "Hash160",
                        value = FindedAsset.AssetHash
                    });
                }
                TypeNValue objs = new TypeNValue()
                {
                    type = "Array",
                    value = AssetPath.ToArray()
                };
                List<TypeNValue> Lists = new List<TypeNValue>()
                {
                    new TypeNValue()
                    {
                        type = "Integer",
                        value = amount
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
                string rawQueryResult = SwapCheck.SwapQuery(queryJson, ConfigReader.GetBestUrl(allNodes));
                ResponseParams queryResult = JsonConvert.DeserializeObject<ResponseParams>(rawQueryResult);
                TypeNValue[] typeNValues = queryResult.result.stack;
                if (typeNValues.Length == 0) continue;
                string result = typeNValues[typeNValues.Length - 1].ToString();
                TypeNValue[] assetAssetmounts = JsonConvert.DeserializeObject<TypeNValue[]>(result);
                List<long> onePathAmountsResult = new List<long>();
                foreach (TypeNValue assetAmount in assetAssetmounts) 
                {
                    onePathAmountsResult.Add(long.Parse(assetAmount.value.ToString()));
                }
                AssetQuery oneQueryResult = new AssetQuery()
                {
                    swapPath = onePath.ToArray(),
                    amount = onePathAmountsResult.ToArray()
                };
                FinalQueryResult.Add(oneQueryResult);              
            }
            return FinalQueryResult;
        }
    }
}
