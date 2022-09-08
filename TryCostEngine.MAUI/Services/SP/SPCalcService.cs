using CostEngine;
using CostEngine.Data.Domain;
using CostEngine.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CostEngine.Data.Extensions;
using TryCostEngine.MAUI.Models;
using static CostEngine.Data.Constants.TPDataConstant;
using static CostEngine.Data.Constants.DataConstant;

namespace TryCostEngine.MAUI.Services
{
    /// <summary>
    /// 用销售政策计算服务
    /// </summary>
    public class SPCalcService : ISPCalcService
    {
        /// <summary>
        /// 费用计算
        /// </summary>
        /// <returns></returns>
        public ApiResultModel<List<CostEngineFeeResult>> FeeCount(string data)
        {
            var r = new ApiResultModel<List<CostEngineFeeResult>>(200, "OK");

            try
            {
                //string data = "";
                var objData = JsonConvert.DeserializeObject<ProtocolBaseModel>(data);

                var joArr = new JArray();

                //基础信息
                if (objData.basicInfo != null)
                {
                    var jo = new JObject
                    {
                        { "lable", "基础信息" },
                        { "terminal_name", objData.basicInfo.TmnNm },
                        { "start_day", objData.basicInfo.StartDate },
                        { "end_day", objData.basicInfo.EndDate },
                        { "agent_1st", objData.basicInfo.Distributor1CD },
                        { "agent_2nd", objData.basicInfo.Distributor2CD ?? "" },
                        { "months", objData.basicInfo.ProtocolMonths },
                        { "company_fee_rate", "0" },
                        { "agent1st_fee_rate", "0" },
                        { "agent2nd_fee_rate", "" }
                    };
                    joArr.Add(jo);
                }

                //产品信息
                if (objData.goodInfo != null)
                {
                    foreach (var goodItem in objData.goodInfo)
                    {
                        var jo = new JObject
                        {
                            { "lable", "销量" },
                            { "prod_id", goodItem.GoodCD },
                            { "prod_name", goodItem.GoodNm },
                            { "sales_amount", string.Join(",", goodItem.Volumes.Select(s => s.YuguSales).ToList()) }
                        };
                        joArr.Add(jo);
                    }
                }

                var goodInfo = objData.goodInfo.Select(b => new SelectListModel
                {
                    Text = b.GoodNm,
                    Value = b.GoodCD,
                    Data = "" + b.OpenFare
                }).ToList();


                //判断名义开票价
                bool isZeroOpenFare = false;
                string errorMsg = "";
                foreach (var obj in objData.goodInfo)
                {
                    if (Convert.ToDecimal(obj.OpenFare) != 0)
                    {
                        isZeroOpenFare = true;
                    }
                    else
                    {
                        errorMsg = errorMsg + obj.GoodNm + " 未配置名义开票价;";
                    }
                }
                if (!isZeroOpenFare)
                {
                    r = new ApiResultModel<List<CostEngineFeeResult>>(500, errorMsg);
                }

                #region 判断投入方式

                if (objData.slfzhTR != null)
                {
                    //随量非组合 
                    joArr.Merge(CostEngineFee.CreateJsonArraySlfzh(objData.slfzhTR, goodInfo));
                }
                if (objData.fydlfzhTR != null)
                {
                    //分月达量非组合
                    joArr.Merge(CostEngineFee.CreateJsonArrayFydlfzh(objData.fydlfzhTR, goodInfo));
                }
                if (objData.ljdlfzhTR != null)
                {
                    //累计达量非组合
                    joArr.Merge(CostEngineFee.CreateJsonArrayLjdlfzh(objData.ljdlfzhTR, goodInfo));
                }
                if (objData.tdfzhTR != null)
                {
                    //梯度达量非组合
                    joArr.Merge(CostEngineFee.CreateJsonArrayTdfzh(objData.tdfzhTR, goodInfo));
                }
                if (objData.slzhTR != null)
                {
                    //随量组合
                    joArr.Merge(CostEngineFee.CreateJsonArraySlzh(objData.slzhTR, goodInfo));
                }
                if (objData.fydlzhTR != null)
                {
                    //分月达量组合
                    joArr.Merge(CostEngineFee.CreateJsonArrayFydlzh(objData.fydlzhTR, goodInfo));
                }
                if (objData.ljdlzhTR != null)
                {
                    //累计达量组合
                    joArr.Merge(CostEngineFee.CreateJsonArrayLjdlzh(objData.ljdlzhTR, goodInfo));
                }
                if (objData.tdzhTR != null)
                {
                    //梯度组合
                    joArr.Merge(CostEngineFee.CreateJsonArrayTdzh(objData.tdzhTR, goodInfo));
                }

                #endregion

                #region 以下逻辑替代API请求
                //                string feeUri = "/api/CostEngine/Fee";
                //                var feeClient = new APIHttpClient();
                //                feeClient.BaseAddress = new Uri(CRBConfig.WebApiTPServiceUrl);
                //                HttpResponseMessage feeResponse = await feeClient.PostAsJsonAsync(feeUri, joArr);
                //                feeResponse.EnsureSuccessStatusCode();
                //#if DEBUG
                //                string json = joArr.ToString();
                //                var debugjsonstring = HttpUtility.UrlEncode(System.Text.Encoding.UTF8.GetBytes(json));
                //                Response.AppendHeader("Debug-Jsonstring", debugjsonstring);
                //#endif
                //                List<CostEngineFee> feeData = await feeResponse.Content.ReadAsAsync<List<CostEngineFee>>();
                //                List<CostEngineFeeResult> result = CostEngineFee.getResult(feeData, objData);
                //                return Json(result);

                //解析费用模型，引擎计算
                //调用梯度模型里的GetCost方法每个月的费用
                //对象模型：
                //->每个协议有多个投入方式
                //->每个投入方式有多个产品，每个产品都有自己的梯度 
                //->每个产品有多月的销量
                #endregion

                #region

                TP_CostEntry cc;
                List<CostResults> costDetails = new();

                string message = "";

                try
                {
                    cc = new TP_CostEntry();
                    JObject[] jos = joArr.Select(s => new JObject(s.Value<JObject>())).ToArray();
                    message = cc.ParseJsonParameter(jos);
                    message = cc.calcCost();
                    costDetails = cc.getJsonResults();
                }
                catch (Exception e)
                {
                    message = e.Message + e.ToString();
                }


                var feeData = costDetails.Select(s => new CostEngineFee
                {
                    //费用类型
                    CostType = s.CostType,
                    //投入编号
                    PromotionID = s.PromotionID,
                    //产品编码
                    ProductID = s.ProductID,
                    //产品名称
                    ProductName = s.ProductName,
                    //按月费用/按月件数
                    CostbyMonth = s.CostbyMonth,
                }).ToList();


                var result = CostEngineFee.getResult(feeData, objData);
                r = new ApiResultModel<List<CostEngineFeeResult>>(200, "Success", result);

                #endregion

            }
            catch (Exception ex)
            {
                r = new ApiResultModel<List<CostEngineFeeResult>>(500, ex.Message);
            }

            return r;
        }


        /// <summary>
        /// 利润测算
        /// </summary>
        /// <param name="infoData"></param>
        /// <param name="feeData"></param>
        /// <returns></returns>
        public ApiResultModel<List<CostEngineProfit>> ProfitCount(string infoData, string feeData)
        {
            var r = new ApiResultModel<List<CostEngineProfit>>(200, "OK");

            try
            {
                var info = JsonConvert.DeserializeObject<ProtocolBaseModel>(infoData); ;
                var fee = JsonConvert.DeserializeObject<List<CostEngineFeeResult>>(feeData);

                int year = DateTime.Now.Year;
                double fixFee = fee.Where(w => w.GivingWayType == InvestPresentMode.固定给钱 || w.GivingWayType == InvestPresentMode.固定给酒)
                    .Sum(s => s.CostCount);
                double companyFee = fee.Sum(s => s.CostCompanyCount);

                var amountList = new List<ProfitProductInfoModel>();
                foreach (var feeItem in fee)
                {
                    foreach (var feeProduct in feeItem.Products)
                    {
                        var goodSale = info.goodInfo.FirstOrDefault(w => w.GoodCD == feeProduct.ProductID);
                        var amountItem = new ProfitProductInfoModel();
                        amountItem.InvestmentNo = feeItem.PromotionID;
                        amountItem.ProductName = feeProduct.ProductName;
                        amountItem.ProductID = feeProduct.ProductID;
                        amountItem.EstimatedTotalSales = goodSale == null ? 0 : goodSale.EstimatedTotalSales;
                        amountItem.TerminalEstimate = Convert.ToDecimal(goodSale == null ? "0" : goodSale.EstimatePrice);
                        amountItem.Fee = Math.Round(feeProduct.CostCount, 2);
                        amountItem.CompanyFee = Math.Round(feeProduct.CostCompanyCount, 2);
                        amountItem.DtLevel1Fee = Math.Round((feeProduct.CostCount - feeProduct.CostCompanyCount) * feeItem.RateDistributor1, 2);
                        amountItem.DtLevel2Fee = Math.Round((feeProduct.CostCount - feeProduct.CostCompanyCount) * feeItem.RateDistributor2, 2);
                        amountList.Add(amountItem);
                    }
                    if (feeItem.Gifts != null)
                    {
                        foreach (var gift in feeItem.Gifts)
                        {
                            var goodSale = info.goodInfo.FirstOrDefault(w => w.GoodCD == gift.ProductID);
                            var amountItem = new ProfitProductInfoModel();
                            amountItem.InvestmentNo = '$' + feeItem.PromotionID;
                            amountItem.ProductName = gift.ProductName;
                            amountItem.ProductID = gift.ProductID;
                            amountItem.GiftCount = gift.GiftCount;
                            amountItem.EstimatedTotalSales = goodSale == null ? 0 : goodSale.EstimatedTotalSales;
                            amountList.Add(amountItem);
                        }
                    }
                }
                #region 获取利润测算公式


                //string queryUri = "/api/TP/ProtocolAdd/FindFormulaByYear";
                //var queryModel = new TPProtocolSearchQueryModel();
                //queryModel.RegionCD = loginUser.CurrentPost.RegionCD;
                //queryModel.MarketOrgCD = loginUser.CurrentPost.MarketOrgCD;
                //queryModel.Year = "" + year;

                //var client = new APIHttpClient();
                //client.BaseAddress = new Uri(CRBConfig.WebApiTPServiceUrl);
                //HttpResponseMessage response = await client.PostAsJsonAsync(queryUri, queryModel);
                //response.EnsureSuccessStatusCode();


                //ApiResultModel<string> formulaResult = await response.Content.ReadAsAsync<ApiResultModel<string>>();
                //if (formulaResult.Code == 500)
                //{
                //    return Json(new ApiResultModel<object>(500, formulaResult.Message));
                //}
                //if (formulaResult.Data == "")
                //{
                //    return Json(new ApiResultModel<object>(500, "未找到利润测算公式"));
                //}

                #endregion

                #region 获取产品信息
                var goodList = info.goodInfo.Select(s => new { s.GoodCD, s.GoodNm, s.DTCD }).ToList();
                #region 将赠品当产品算利润
                if (info.slfzhTR != null)
                {
                    foreach (var data in info.slfzhTR.data)
                    {
                        if (data.GivingWayType == InvestPresentMode.促销用酒)
                        {
                            goodList.AddRange(data.GiveGood.Select(s => new { s.GoodCD, s.GoodNm, DTCD = s.DTCD }).ToList());
                        }
                    }
                }
                if (info.fydlfzhTR != null)
                {
                    foreach (var data in info.fydlfzhTR.data)
                    {
                        if (data.GivingWayType == InvestPresentMode.促销用酒)
                        {
                            goodList.AddRange(data.GiveGood.Select(s => new { s.GoodCD, s.GoodNm, DTCD = s.DTCD }).ToList());
                        }
                        if (data.GivingWayType == InvestPresentMode.固定给酒)
                        {
                            goodList.AddRange(data.FixGood.Select(s => new { s.GoodCD, s.GoodNm, DTCD = s.DTCD }).ToList());
                        }
                    }
                }
                if (info.ljdlfzhTR != null)
                {
                    foreach (var data in info.ljdlfzhTR.data)
                    {
                        if (data.GivingWayType == InvestPresentMode.促销用酒)
                        {
                            goodList.AddRange(data.GiveGood.Select(s => new { s.GoodCD, s.GoodNm, DTCD = s.DTCD }).ToList());
                        }
                        if (data.GivingWayType == InvestPresentMode.固定给酒)
                        {
                            goodList.AddRange(data.FixGood.Select(s => new { s.GoodCD, s.GoodNm, DTCD = s.DTCD }).ToList());
                        }
                    }
                }
                if (info.tdfzhTR != null)
                {
                    foreach (var data in info.tdfzhTR.data)
                    {
                        if (data.GivingWayType == InvestPresentMode.促销用酒)
                        {
                            goodList.AddRange(data.GiveGood.Select(s => new { s.GoodCD, s.GoodNm, DTCD = s.DTCD }).ToList());
                        }
                    }
                }
                if (info.gdgjTR != null)
                {
                    foreach (var data in info.gdgjTR.data)
                    {
                        goodList.Add(new { data.GoodCD, data.GoodNm, DTCD = data.DTCD });
                    }
                }
                if (info.slzhTR != null)
                {
                    foreach (var data in info.slzhTR.data)
                    {
                        if (data.GivingWayType == InvestPresentMode.促销用酒)
                        {
                            goodList.AddRange(data.Details.SelectMany(sm => sm.GiveGood.Select(s => new { s.GoodCD, s.GoodNm, DTCD = s.DTCD }).ToList()));
                        }
                    }
                }
                if (info.fydlzhTR != null)
                {
                    foreach (var data in info.fydlzhTR.data)
                    {
                        if (data.GivingWayType == InvestPresentMode.促销用酒 || data.GivingWayType == InvestPresentMode.固定给酒)
                        {
                            goodList.AddRange(data.Details.SelectMany(sm => sm.GiveGood.Select(s => new { s.GoodCD, s.GoodNm, DTCD = s.DTCD }).ToList()));
                        }
                    }
                }
                if (info.ljdlzhTR != null)
                {
                    foreach (var data in info.ljdlzhTR.data)
                    {
                        if (data.GivingWayType == InvestPresentMode.促销用酒 || data.GivingWayType == InvestPresentMode.固定给酒)
                        {
                            goodList.AddRange(data.Details.SelectMany(sm => sm.GiveGood.Select(s => new { s.GoodCD, s.GoodNm, DTCD = s.DTCD }).ToList()));
                        }
                    }
                }
                if (info.tdzhTR != null)
                {
                    foreach (var data in info.tdzhTR.data)
                    {
                        if (data.GivingWayType == InvestPresentMode.促销用酒)
                        {
                            goodList.AddRange(data.Details.SelectMany(sm => sm.GiveGood.Select(s => new { s.GoodCD, s.GoodNm, DTCD = s.DTCD }).ToList()));
                        }
                    }
                }
                #endregion

                var infoResult = new ApiResultModel<List<ProfitProductInfoModel>>(200, "OK");

                //string infoUri = "/api/TP/ProtocolAdd/GetProductProfitInfo?regionCD=" + loginUser.CurrentPost.RegionCD +
                //    "&marketOrgCD=" + loginUser.CurrentPost.MarketOrgCD +
                //    "&stationOrgCD=" + loginUser.CurrentPost.StationOrgCD +
                //    "&dtCD=" + info.basicInfo.Distributor1CD +
                //    "&tmnCD=" + info.basicInfo.TmnCD +
                //    "&startDate=" + info.basicInfo.StartDate +
                //    "&productCDs=" + string.Join(",", goodList.Select(s => s.GoodCD + "|" + s.DTCD).Distinct().ToList());
                //var infoClient = new APIHttpClient();
                //infoClient.BaseAddress = new Uri(CRBConfig.WebApiTPServiceUrl);
                //HttpResponseMessage infoResponse = await client.GetAsync(infoUri);
                //infoResponse.EnsureSuccessStatusCode();
                //var infoResult = await infoResponse.Content.ReadAsAsync<ApiResultModel<List<ProfitProductInfoModel>>>();
                //if (infoResult.Code == 500)
                //{
                //    return Json(new ApiResultModel<object>(500, infoResult.Message));
                //}
                //var KLConvRate = infoResult.Data.Where(w => w.KLConvRate == 0).Select(s => s.ProductID).ToList();
                //if (KLConvRate.Count > 0)
                //{
                //    return Json(new ApiResultModel<object>(500, $"产品{string.Join(",", KLConvRate)}未维护千升转换率；"));
                //}

                #endregion

                foreach (var data in infoResult.Data)
                {
                    var isExist = false;
                    foreach (var amount in amountList)
                    {
                        if (amount.ProductID == data.ProductID)
                        {
                            isExist = true;
                            amount.OpenFare = data.OpenFare;
                            amount.MeasureRate = data.MeasureRate;
                            amount.CostChanged = data.CostChanged;
                            amount.KLConvRate = data.KLConvRate;
                            amount.EntryPrice = data.EntryPrice;
                            amount.ProfitRet = data.ProfitRet;
                            amount.SingleStd = data.SingleStd;
                            amount.StationOrg = data.StationOrg;
                        }
                    }
                    if (!isExist)
                    {
                        var goodSale = info.goodInfo.FirstOrDefault(w => w.GoodCD == data.ProductID);
                        var amountItem = new ProfitProductInfoModel
                        {
                            ProductName = data.ProductName,
                            ProductID = data.ProductID,
                            OpenFare = data.OpenFare,
                            MeasureRate = data.MeasureRate,
                            CostChanged = data.CostChanged,
                            KLConvRate = data.KLConvRate,
                            EntryPrice = data.EntryPrice,
                            ProfitRet = data.ProfitRet,
                            SingleStd = data.SingleStd,
                            StationOrg = data.StationOrg,
                            EstimatedTotalSales = goodSale == null ? 0 : goodSale.EstimatedTotalSales,
                            TerminalEstimate = Convert.ToDecimal(goodSale == null ? 0 : goodSale.EstimatePrice)
                        };
                        amountList.Add(amountItem);
                    }
                }
                #region 进店价用页面的数据
                foreach (var amount in amountList)
                {
                    var goods = info.goodInfo.FirstOrDefault(f => f.GoodCD == amount.ProductID);
                    if (goods != null)
                    {
                        amount.EntryPrice = decimal.Parse(goods.EntryPrice);
                        amount.TerminalEstimate = decimal.Parse(goods.EstimatePrice);
                    }
                }
                #endregion

                var profitData = new List<CostEngineProfit>();

                //                JArray joArr = CreateProfitJson(info, loginUser, year, fixFee, companyFee, amountList, formulaResult.Data);
                //#if DEBUG
                //                string json = joArr.ToString();
                //                Response.AppendHeader("Debug-Jsonstring", json);
                //#endif
                //                string profitUri = "/api/CostEngine/Profit";
                //                var profitClient = new APIHttpClient();
                //                profitClient.BaseAddress = new Uri(CRBConfig.WebApiTPServiceUrl);
                //                HttpResponseMessage profitResponse = await profitClient.PostAsJsonAsync(profitUri, joArr);
                //                if (profitResponse.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                //                {
                //                    CostEngineProfitException exception = await profitResponse.Content.ReadAsAsync<CostEngineProfitException>();
                //                    return Json(new ApiResultModel<object>(500, exception.ExceptionMessage));
                //                }
                //                List<CostEngineProfit> profitData = await profitResponse.Content.ReadAsAsync<List<CostEngineProfit>>();
                //                if (profitData.Count < 1)
                //                {
                //                    return Json(new ApiResultModel<object>(500, "利润测算失败"));
                //                }



                foreach (var product in profitData)
                {
                    if (product.ProductCD.StartsWith("$"))
                    {
                        product.ProductCD = product.ProductCD.Substring(1);
                        var good = goodList.FirstOrDefault(w => w.GoodCD == product.ProductCD);
                        var goods = infoResult.Data.FirstOrDefault(f => f.ProductID == product.ProductCD);
                        var amount = amountList.FirstOrDefault(f => f.ProductID == product.ProductCD);
                        product.ProductName = "（赠）" + good.GoodNm;
                        product.ProfitRet = amount.ProfitRet;
                        product.ProductSingleStd = amount.SingleStd;
                    }
                    else if (product.ProductCD != "--")
                    {
                        var good = goodList.Where(w => w.GoodCD == product.ProductCD).FirstOrDefault();
                        var goods = infoResult.Data.FirstOrDefault(f => f.ProductID == product.ProductCD);
                        var goodsAmount = amountList.Where(w => w.ProductID == product.ProductCD).FirstOrDefault();
                        var amount = amountList.FirstOrDefault(f => f.ProductID == product.ProductCD);
                        product.ProductName = good.GoodNm;
                        product.ProfitRet = amount.ProfitRet;
                        product.ProductSingleStd = amount.SingleStd;
                    }
                }

                //return Json(new ApiResultModel<List<CostEngineProfit>>(200, "测算成功", profitData));
            }
            catch (Exception ex)
            {
                r = new ApiResultModel<List<CostEngineProfit>>(500, ex.Message);
            }
            return r;
        }
    }
}
