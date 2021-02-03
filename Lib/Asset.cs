using System;
using System.Collections.Generic;

namespace Lib
{
    public class Asset
    {
        public string AssetName;
        public string AssetHash;
        //public List<Asset> ExchangeableAsset;

        public Asset() { }
        public Asset(string assetName, string assetHash) 
        {
            AssetName = assetName;
            AssetHash = assetHash;
            //ExchangeableAsset = new List<Asset>();
        }

        //public void AddExchangeableAsset(Asset asset) 
        //{
        //    ExchangeableAsset.Add(asset);
        //}
    }
}
