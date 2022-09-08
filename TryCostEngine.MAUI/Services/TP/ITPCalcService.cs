namespace TryCostEngine.MAUI.Services
{
    using System.Collections.Generic;
    using TryCostEngine.MAUI.Models;
    using CostEngine.Data.Models;

    /// <summary>
    /// 用销售政策计算服务
    /// </summary>
    public interface ITPCalcService
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