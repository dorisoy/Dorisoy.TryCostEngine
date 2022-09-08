using System;
using System.Collections.Generic;
using CostEngine;
using CostEngine.Data;
using CostEngine.Data.Models;
using CostEngine.Util;
using CostEngine.Data.Domain;
using TryCostEngine.MAUI.Models;

namespace TryCostEngine.MAUI.Services
{
    public interface IMenuService
    {
        IEnumerable<MudComponent> Components { get; }
        IEnumerable<MudComponent> TPModels { get; }
        IEnumerable<MudComponent> SPModels { get; }

        IEnumerable<MudComponent> Api { get; }

        MudComponent GetParent(Type type);
        MudComponent GetComponent(Type type);


        MudComponent GetModelParent(Type type);
        MudComponent GetModel(Type type);
    }

    public class MenuService : IMenuService
    {

        public IEnumerable<MudComponent> Components => _docsComponents;
        private readonly List<MudComponent> _docsComponents = new DocsComponents()

            //Config
            .AddNavGroup("配置", false, new DocsComponents()
                .AddItem("ET_ConfigCalcData", typeof(ET_ConfigCalcData))
                .AddItem("ET_ConfigCostTax", typeof(ET_ConfigCostTax))
                .AddItem("ET_ConfigTaxRatio", typeof(ET_ConfigTaxRatio))
                .AddItem("ET_M_PriceSys", typeof(ET_M_PriceSys))
            )

            //Entity
            .AddNavGroup("实体", false, new DocsComponents()
                .AddItem("ExpressionCalc", typeof(ExpressionCalc))
                .AddItem("FormulaCalc", typeof(FormulaCalc))
            )

            //Util
            .AddNavGroup("公共", false, new DocsComponents()
                .AddItem("M_PriceSys", typeof(M_PriceSys))
            )

            //Model
            .AddNavGroup("基类", false, new DocsComponents()
                .AddItem("CostEntry", typeof(CostEntry))
                .AddItem("PromotionsCosts", typeof(PromotionsCosts))
                .AddItem("BasicInfo", typeof(BasicInfo))
                .AddItem("Beer", typeof(Beer))
                .AddItem("CostResults", typeof(CostResults))
                .AddItem("GiftBeer", typeof(GiftBeer))
                .AddItem("Productinfo", typeof(Productinfo))
                .AddItem("ProductsPackage", typeof(ProductsPackage))
                .AddItem("Stage", typeof(Stage))
                .AddItem("SubsidyStage", typeof(SubsidyStage))


                .AddItem("FmBaseMode", typeof(FmBaseMode))
                .AddItem("ProtocolBaseModel", typeof(ProtocolBaseModel))
                .AddItem("AddProtocolAttachment", typeof(AddProtocolAttachment))
                .AddItem("DeleteProtocolAttachment", typeof(DeleteProtocolAttachment))
                .AddItem("ProtocolBaseBasicinfo", typeof(ProtocolBaseBasicinfo))
                .AddItem("ProtocolBaseGoodinfo", typeof(ProtocolBaseGoodinfo))
                .AddItem("ProtocolBaseVolume", typeof(ProtocolBaseVolume))
                .AddItem("ProtocolModificationReplaceProduct", typeof(ProtocolModificationReplaceProduct))
                .AddItem("ViewProtocolFeeCount", typeof(ViewProtocolFeeCount))
                .AddItem("ViewProtocolWarning", typeof(ViewProtocolWarning))
                .AddItem("ProtocolBaseCosts", typeof(ProtocolBaseCosts))
                .AddItem("ProtocolBaseFee", typeof(ProtocolBaseFee))
            )

            ////Profit
            //.AddNavGroup("利润测算", false, new DocsComponents()
            //    .AddItem("BaseProfit", typeof(BaseProfit))
            //    .AddItem("IProfit", typeof(IProfit))
            //    .AddItem("ProfitEntryNew", typeof(ProfitEntryNew))
            //    .AddItem("ProfitEntryXian", typeof(ProfitEntryXian))
            //    .AddItem("ProfitResults", typeof(ProfitResults))
            //)

            ////SP
            //.AddNavGroup("政策", false, new DocsComponents()
            //    .AddItem("SP_Costs", typeof(SP_Costs))
            //    .AddItem("SP_Entry", typeof(SP_Entry))
            //    .AddItem("SP_ProfitEntry", typeof(SP_ProfitEntry))
            //    .AddItem("SP_ProfitResult", typeof(SP_ProfitResult))
            //    .AddItem("SP_Results", typeof(SP_Results))
            //)

            ////SP2
            //.AddNavGroup("销售政策梯组", false, new DocsComponents()
            //    .AddItem("SP_Costs2", typeof(SP_Costs2))
            //    .AddItem("SP_Entry2", typeof(SP_Entry2))
            //    .AddItem("SP_SubsidyStage2", typeof(SP_SubsidyStage2))
            //)

            //TP
            .AddNavGroup("协议", false, new DocsComponents()
                .AddItem("TP_CostEntry", typeof(TP_CostEntry))
                .AddItem("TP_Costs", typeof(TP_Costs))
                .AddItem("TP_ProfitEntry", typeof(TP_ProfitEntry))
                .AddItem("TP_ProfitResult", typeof(TP_ProfitResult))
            )


            //分月达量
            .AddNavGroup("分月达量", false, new DocsComponents()
            .AddItem("ProtocolBaseFydlfzh", typeof(ProtocolBaseFydlfzh))
            .AddItem("ProtocolBaseFydlfzhDatum", typeof(ProtocolBaseFydlfzhDatum))
            .AddItem("ProtocolBaseFixGood", typeof(ProtocolBaseFixGood))
            .AddItem("ProtocolBaseReqAmounts", typeof(ProtocolBaseReqAmounts))

            .AddItem("ProtocolBaseFydlzh", typeof(ProtocolBaseFydlzh))
            .AddItem("ProtocolBaseFydlzhDatum", typeof(ProtocolBaseFydlzhDatum))

            ,true)

            //固定
            .AddNavGroup("固定", false, new DocsComponents()
            //固定给酒
            .AddItem("ProtocolBaseGdgj", typeof(ProtocolBaseGdgj))
            .AddItem("ProtocolBaseGdgjDatum", typeof(ProtocolBaseGdgjDatum))
            .AddItem("ProtocolBaseGdgjPayrate", typeof(ProtocolBaseGdgjPayrate))
            .AddItem("ProtocolBaseGdgjPaymonthsc2d", typeof(ProtocolBaseGdgjPaymonthsc2d))
            //固定给钱
            .AddItem("ProtocolBaseGdgq", typeof(ProtocolBaseGdgq))
             .AddItem("ProtocolBaseGdgqDatum", typeof(ProtocolBaseGdgqDatum))
             .AddItem("ProtocolBaseGdgqPayrate", typeof(ProtocolBaseGdgqPayrate))
             .AddItem("ProtocolBaseGdgqPaymonths", typeof(ProtocolBaseGdgqPaymonths))
             .AddItem("ProtocolBaseFeebear", typeof(ProtocolBaseFeebear))
             .AddItem("ProtocolBaseInvestdetail", typeof(ProtocolBaseInvestdetail))
            , true)

            //累计达量
            .AddNavGroup("累计达量", false, new DocsComponents()
            //分月达量非组合
            .AddItem("ProtocolBaseLjdlfzh", typeof(ProtocolBaseLjdlfzh))
            .AddItem("ProtocolBaseLjdlfzhDatum", typeof(ProtocolBaseLjdlfzhDatum))
            //分月达量组合
            .AddItem("ProtocolBaseLjdlzh", typeof(ProtocolBaseLjdlzh))
            , true)

            //随量
            .AddNavGroup("随量", false, new DocsComponents()
            //随量非组合
            .AddItem("ProtocolBaseSlfzh", typeof(ProtocolBaseSlfzh))
            .AddItem("ProtocolBaseSlfzhDatum", typeof(ProtocolBaseSlfzhDatum))
            .AddItem("ProtocolBaseRebate", typeof(ProtocolBaseRebate))
            .AddItem("ProtocolBaseBearway", typeof(ProtocolBaseBearway))
            .AddItem("ProtocolBasePayrate", typeof(ProtocolBasePayrate))
            .AddItem("ProtocolBasePaymonths", typeof(ProtocolBasePaymonths))
            .AddItem("ProtocolBaseGivegood", typeof(ProtocolBaseGivegood))
            //随量组合
            .AddItem("ProtocolBaseSlzh", typeof(ProtocolBaseSlzh))
            .AddItem("ProtocolBaseSlzhDatum", typeof(ProtocolBaseSlzhDatum))
            .AddItem("ProtocolBaseDetail", typeof(ProtocolBaseDetail))
            .AddItem("ProtocolBaseSet", typeof(ProtocolBaseSet))
            , true)

            //梯度
            .AddNavGroup("梯度", false, new DocsComponents()
            //梯度非组合
            .AddItem("ProtocolBaseTdfzh", typeof(ProtocolBaseTdfzh))
            .AddItem("ProtocolBaseTdfzhDatum", typeof(ProtocolBaseTdfzhDatum))
            .AddItem("ProtocolBaseTdfzhGivegood", typeof(ProtocolBaseTdfzhGivegood))
            .AddItem("ProtocolBaseTdfzhBearWay", typeof(ProtocolBaseTdfzhBearWay))
            .AddItem("ProtocolBaseTdfzhGiveAmountStep", typeof(ProtocolBaseTdfzhGiveAmountStep))
            .AddItem("ProtocolBaseTdfzhRebate", typeof(ProtocolBaseTdfzhRebate))
            .AddItem("ProtocolBaseTdfzhFee", typeof(ProtocolBaseTdfzhFee))
            .AddItem("ProtocolBaseTdfzhBaseNum", typeof(ProtocolBaseTdfzhBaseNum))
            .AddItem("ProtocolBaseStep", typeof(ProtocolBaseStep))
            //梯度组合
            .AddItem("ProtocolBaseTdzh", typeof(ProtocolBaseTdzh))
            .AddItem("ProtocolBaseTdzhDatum", typeof(ProtocolBaseTdzhDatum))
            .AddItem("ProtocolBaseTdzhDetail", typeof(ProtocolBaseTdzhDetail))
            , true)
            //
            .GetComponentsSortedByName();



        private Dictionary<Type, MudComponent> _parents = new();
        private Dictionary<Type, MudComponent> _componentLookup = new();
        public MudComponent GetParent(Type child) {
            if (child == null)
                return null;
            if (_parents.TryGetValue(child, out var parent))
                return parent;
            return null;
        }
        public MudComponent GetComponent(Type type)
        {
            if (type == null)
                return null;
            if (_componentLookup.ContainsKey(type))
                return _componentLookup[type];
            if (_parents.ContainsKey(type))
                return _parents[type];
            return null;
        }

        public MenuService()
        {
            foreach (var comp in Components)
            {
                if (comp.IsNavGroup)
                {
                    foreach (var groupComp in comp.GroupComponents)
                    {
                        _componentLookup.Add(groupComp.Type, groupComp);
                        _parents.Add(groupComp.Type, comp);
                    }
                }
                else
                {
                    _componentLookup.Add(comp.Type, comp);
                    // top-level types refer to themself as parent ;)
                    _parents.Add(comp.Type, comp);
                    if (comp.ChildTypes != null)
                    {
                        foreach (var childType in comp.ChildTypes)
                            _parents.Add(childType, comp);
                    }
                }
            }

            //foreach (var comp in Models)
            //{
            //    if (comp.IsNavGroup)
            //    {
            //        foreach (var groupComp in comp.GroupComponents)
            //        {
            //            _modelLookup.Add(groupComp.Type, groupComp);
            //            _modelParents.Add(groupComp.Type, comp);
            //        }
            //    }
            //    else
            //    {
            //        _modelLookup.Add(comp.Type, comp);
            //        _modelParents.Add(comp.Type, comp);
            //        if (comp.ChildTypes != null)
            //        {
            //            foreach (var childType in comp.ChildTypes)
            //                _modelParents.Add(childType, comp);
            //        }
            //    }
            //}
        }


        private DocsComponents _docsComponentsApi;
        private DocsComponents DocsComponentsApi
        {
            get
            {

                if (_docsComponentsApi != null)
                    return _docsComponentsApi;

                _docsComponentsApi = new DocsComponents();

                foreach (var item in Components)
                {
                    if (item.IsNavGroup)
                    {
                        foreach (var groupComp in item.GroupComponents)
                        {
                            _docsComponentsApi.AddItem(groupComp.Name, groupComp.Type);
                        }
                    }
                    else
                    {
                        _docsComponentsApi.AddItem(item.Name, item.Type);
                    }
                }
                return _docsComponentsApi;
            }
        }
        public IEnumerable<MudComponent> Api => DocsComponentsApi.Components;


        private Dictionary<Type, MudComponent> _modelParents = new();
        private Dictionary<Type, MudComponent> _modelLookup = new();
        public MudComponent GetModelParent(Type child)
        {
            if (child == null)
                return null;
            if (_modelParents.TryGetValue(child, out var parent))
                return parent;
            return null;
        }
        public MudComponent GetModel(Type type)
        {
            if (type == null)
                return null;
            if (_modelLookup.ContainsKey(type))
                return _modelLookup[type];
            if (_modelParents.ContainsKey(type))
                return _parents[type];
            return null;
        }

        public IEnumerable<MudComponent> TPModels => _tpModels;
        private readonly List<MudComponent> _tpModels = new DocsComponents()

            //随量
            .AddNavGroup("随量", false, new DocsComponents()

            .AddItem("随量非组合", typeof(ProtocolBaseSlfzh))
            .AddItem("随量组合", typeof(ProtocolBaseSlzh))

            .AddItem("分月达量非组合", typeof(ProtocolBaseFydlfzh))
            .AddItem("分月达量组合", typeof(ProtocolBaseFydlzh))

            .AddItem("累计达量非组合", typeof(ProtocolBaseLjdlfzh))
            .AddItem("累计达量组合", typeof(ProtocolBaseLjdlzh))

            .AddItem("梯度非组合", typeof(ProtocolBaseTdfzh))
            .AddItem("梯度组合", typeof(ProtocolBaseTdzh))
            )

            //固定
            .AddNavGroup("固定", false, new DocsComponents()
            .AddItem("固定给酒", typeof(ProtocolBaseGdgj))
            .AddItem("固定给钱", typeof(ProtocolBaseGdgq))
            )

            .GetComponentsSortedByName();



        public IEnumerable<MudComponent> SPModels => _spModels;
        private readonly List<MudComponent> _spModels = new DocsComponents()

            //量有关
            .AddNavGroup("量有关", false, new DocsComponents()
            .AddItem("有奖促销", typeof(ProtocolBaseFydlfzh))
            .AddItem("陈列", typeof(ProtocolBaseFydlzh))
            .AddItem("补贴", typeof(ProtocolBaseFydlzh))
            .AddItem("通用政策", typeof(ProtocolBaseFydlzh))
            )

            //量无关
            .AddNavGroup("量无关", false, new DocsComponents()
            .AddItem("随量", typeof(ProtocolBaseGdgj))
            .AddItem("常规", typeof(ProtocolBaseGdgq))
            .AddItem("组合买赠", typeof(ProtocolBaseGdgq))
            .AddItem("梯度买赠", typeof(ProtocolBaseGdgq))
            .AddItem("促销品", typeof(ProtocolBaseGdgq))
            )

            .GetComponentsSortedByName();


    }
}
