namespace TryCostEngine.MAUI.Services
{
    using System.Collections.Generic;
    using TryCostEngine.MAUI.Models;
    using CostEngine.Data.Models;

    /// <summary>
    /// 用于协议终端计算服务
    /// </summary>
    public interface ISPCalcService
    {
        /// <summary>
        /// 费用计算
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        ApiResultModel<List<CostEngineFeeResult>> FeeCount(string data);

        /// <summary>
        /// 利润测算
        /// </summary>
        /// <param name="infoData"></param>
        /// <param name="feeData"></param>
        /// <returns></returns>
        ApiResultModel<List<CostEngineProfit>> ProfitCount(string infoData, string feeData);
    }
}