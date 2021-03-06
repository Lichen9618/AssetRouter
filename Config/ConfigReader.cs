﻿using System;
using System.Xml;
using System.Collections.Generic;
using Lib;

namespace Config
{
    public class ConfigReader        
    {
        XmlDocument AssetConfig;
        XmlDocument ContractConfig;
        XmlDocument SwapPairConfig;
        XmlDocument NodeConfig;
        public ConfigReader() 
        {
            AssetConfig = new XmlDocument();
            ContractConfig = new XmlDocument();
            SwapPairConfig = new XmlDocument();
            NodeConfig = new XmlDocument();
            AssetConfig.Load("Asset.xml");
            SwapPairConfig.Load("SwapPair.xml");
            ContractConfig.Load("Contract.xml");
            NodeConfig.Load("NodeConfig.xml");
        }

        public List<Asset> GetAllAsset() 
        {
            XmlNode AllAsset = AssetConfig.SelectSingleNode("AllAsset");
            XmlNodeList AssetsXML = AllAsset.ChildNodes;
            List<Asset> resultAssets = new List<Asset>();
            foreach (var assetXML in AssetsXML)
            {
                XmlElement assetElement = (XmlElement)assetXML;
                var assetXmlNodes = assetElement.ChildNodes;
                Asset asset = new Asset(assetXmlNodes.Item(0).InnerText, assetXmlNodes.Item(1).InnerText);                
                resultAssets.Add(asset);
            }
            return resultAssets;
        }

        public Contract GetCallContract() 
        {
            XmlNode ContractCall = ContractConfig.SelectSingleNode("Contract");
            var ContractInfos = ContractCall.ChildNodes;
            Contract contractResult = new Contract()
            {
                ContractHash = ContractInfos.Item(1).InnerText,
                FunctionName = ContractInfos.Item(2).InnerText
            };
            return contractResult;
        }

        public List<SwapPair> GetAllSwapPair() 
        {
            List<Asset> AssetList = GetAllAsset();
            XmlNode SwapPairNode = SwapPairConfig.SelectSingleNode("AllPair");
            XmlNodeList PairLists = SwapPairNode.ChildNodes;
            List<SwapPair> resultSwapPairs = new List<SwapPair>();
            foreach (var Pair in PairLists) 
            {
                XmlElement pairElement = (XmlElement)Pair;
                var infoNodes = pairElement.ChildNodes;
                SwapPair newPair = new SwapPair()
                {
                    StartAsset = AssetList.Find(T => T.AssetName.Equals(infoNodes.Item(0).InnerText)),
                    EndAsset = AssetList.Find(T => T.AssetName.Equals(infoNodes.Item(1).InnerText))
                };
                resultSwapPairs.Add(newPair);
            }
            return resultSwapPairs;
        }

        public List<string> GetAllNodeUrl() 
        {
            XmlNode AllAsset = NodeConfig.SelectSingleNode("AllNodes");
            XmlNodeList AssetsXML = AllAsset.ChildNodes;
            List<string> resultNodes = new List<string>();
            foreach (var assetXML in AssetsXML)
            {
                XmlElement assetElement = (XmlElement)assetXML;
                var assetXmlNodes = assetElement.ChildNodes;
                string asset = assetXmlNodes.Item(0).InnerText;
                resultNodes.Add(asset);
            }
            return resultNodes;
        }

        public static string GetBestUrl(List<string> Nodes) 
        {
            //TODO: 后期优化节点选择
            return Nodes[0];
        }
    }
}
