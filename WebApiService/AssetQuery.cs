using System;
using Lib;
using System.Numerics;
using System.Collections.Generic;

namespace WebAPIService
{
    public class AssetQuery
    {
        public string[] swapPath { get; set; }
        public long[] amount { get; set; }
    }
}
