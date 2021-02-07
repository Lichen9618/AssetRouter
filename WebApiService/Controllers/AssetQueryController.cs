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

namespace WebAPIService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AssetQueryController : ControllerBase
    {
        private readonly ILogger<AssetQueryController> _logger;

        public AssetQueryController(ILogger<AssetQueryController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public string AssetPathQuery(string StartAsset, string EndAsset, int amount)
        {
            List<AssetQuery> FinalQueryResult = new List<AssetQuery>();

            ConfigReader config = new ConfigReader();
            var assetList = config.GetAllAsset();
            var CallContract = config.GetCallContract();
            var SwapPairs = config.GetAllSwapPair();
            Graph<string, int> graph = new Graph<string, int>();
            foreach (var pair in SwapPairs)
            {
                graph.AddEdge(pair.StartAsset.AssetName, pair.EndAsset.AssetName, 1);
            }
            DirectedGraph.LinkedList<DirectedGraph.LinkedList<Node<string, int>>> results = graph.Search(StartAsset, EndAsset);
            foreach (DirectedGraph.LinkedList<Node<string, int>> path in results)
            {
                //对每一条path进行一条rpc查询
                List<TypeNValue> AssetPath = new List<TypeNValue>();
                List<string> onePath = new List<string>();
                foreach (Node<string, int> Asset in path)
                {
                    var FindedAsset = assetList.Find(T => T.AssetName.Equals(Asset.Value));
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
                var Lists = new List<TypeNValue>()
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
                string rawQueryResult = SwapCheck.SwapQuery(queryJson);
                var queryResult = JsonConvert.DeserializeObject<ResponseParams>(rawQueryResult);
                TypeNValue[] typeNValues = queryResult.result.stack;
                if (typeNValues.Length == 0) continue;
                var result = (typeNValues[typeNValues.Length - 1].ToString());
                TypeNValue[] assetAmounts = JsonConvert.DeserializeObject<TypeNValue[]>(result);
                Console.WriteLine("------------------Result----------------------");
                List<long> onePathAmountsResult = new List<long>();
                foreach (TypeNValue assetAmount in assetAmounts)
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
            return JsonConvert.SerializeObject(FinalQueryResult);
        }
    }
}
