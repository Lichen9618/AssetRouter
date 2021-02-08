# AssetRouter

## 环境依赖:
.net core 3.1   
.net standard 2.0
https://dotnet.microsoft.com/download/visual-studio-sdks?utm_source=getdotnetsdk&utm_medium=referral

## 配置文件：

Asset.xml
配置资产名与合约哈希

SwapPair.xml
配置合约路径， 每一个Pair表示单向。如需双向， 需要配置两个pair
例：nNEO与pnUSDT可以互换
  <Pair>
    <Start>nNEO</Start>
    <End>pnUSDT</End>
  </Pair>
  <Pair>
    <Start>pnUSDT</Start>
    <End>nNEO</End>
  </Pair>
  
## Api服务启动
windows环境下：
1. 运行WebApiService.exe， 默认5000端口， 请确保不被占用
2. 检查配置文件是否存在（Asset.xml, SwapPair.xml, Contract.xml）
3. 通过本地浏览器访问 http://localhost:5000/swagger/index.html
4. 点击需要调试的Api（当前仅有AssetQuery）, 点击(try it out), 输入参数（StartAsset, EndAsset, amount）,  点击Execute
5. 根据生成的request, response进行调试

备注： 目前支持的资产已在配置文件中配置完毕， 包括 (nNEO, pnUSDT, pnWBTC, pnUSDT, FLM, pONT SWTH)

