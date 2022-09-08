using System.Linq;
using System;
using TryCostEngine.MAUI.Models;
using Microsoft.AspNetCore.Components;
using TryCostEngine.MAUI.Services;
using TryCostEngine.MAUI.Extensions;
using System.Collections.Generic;
using MudBlazor;
using TryCostEngine.MAUI.Pages.Calc.Components;

namespace TryCostEngine.MAUI.Pages
{
    public partial class Unit
    {
        IDialogReference _dialogReference;

        private TreeItemData _ActivatedValue;
        public TreeItemData ActivatedValue
        {
            get { return _ActivatedValue; }
            set
            {
                _ActivatedValue = value;
                if (value != null)
                    OpenDialog();
            }
        }

        private HashSet<TreeItemData> TreeItems { get; set; } = new HashSet<TreeItemData>();

        private void OpenDialog()
        {
            var itemData = ActivatedValue;
            var option = new DialogOptions()
            {
                FullWidth = true,
                MaxWidth = MaxWidth.Large,
                CloseButton = true,
                CloseOnEscapeKey = true
            };
            if (itemData != null && !(itemData?.TreeItems?.Any() ?? false))
            {
                var parameters = new DialogParameters();
                parameters.Add("CalcDes", itemData.Des);
                _dialogReference = Dialog.Show<CalcDialog>(itemData?.Title ?? "测试", parameters, option);
            }
        }

        public class TreeItemData
        {
            public string Title { get; set; }

            public string Icon { get; set; }

            public string Des { get; set; }

            public bool IsExpanded { get; set; } = true;
            public bool HasChild => TreeItems != null && TreeItems.Count > 0;

            public HashSet<TreeItemData> TreeItems { get; set; }

            public TreeItemData(string title, string icon, string des = "")
            {
                Title = title;
                Icon = icon;
                Des = des;
            }
        }

        protected override void OnInitialized()
        {

            #region //固定-固定给钱
            //105
            var gd_gdgj_ajs = new TreeItemData("按件数", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    new TreeItemData("按件数", Icons.Filled.Info, "")
                }
            };
            //104
            var gd_gdgj = new TreeItemData("固定给酒", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //固定-固定给钱-按件数
                    gd_gdgj_ajs
                }
            };
            #endregion

            #region  //固定-固定给钱
            //103
            var gd_gdgq_aje = new TreeItemData("按金额", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    new TreeItemData("按金额", Icons.Filled.Info, "")
                }
            };
            //102
            var gd_gdgq_abfb = new TreeItemData("按百分比", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    new TreeItemData("按百分比", Icons.Filled.Info, "")
                }
            };
            //101
            var gd_gdgq = new TreeItemData("固定给钱", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //固定-固定给钱-按百分比
                    gd_gdgq_abfb,
                    //固定-固定给钱-按金额
                    gd_gdgq_aje,
                }
            };
            #endregion


            #region  //随量-无套装-梯度达量
            //100
            var sl_noPackage_tddl_lj_bfg = new TreeItemData("不分割", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                  new TreeItemData("不分割", Icons.Filled.Label, "")
                }
            };
            //99
            var sl_noPackage_tddl_lj_fg = new TreeItemData("分割", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                  new TreeItemData("分割", Icons.Filled.Label, "")
                }
            };
            //98
            var sl_noPackage_tddl_lj = new TreeItemData("累计", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-无套装-梯度达量-累计-分割
                    sl_noPackage_tddl_lj_fg,
                    //随量-无套装-梯度达量-累计-不分割
                    sl_noPackage_tddl_lj_bfg,
                }
            };
            //97
            var sl_noPackage_tddl_fy_bfg = new TreeItemData("不分割", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                  new TreeItemData("不分割", Icons.Filled.Label, "")
                }
            };
            //96
            var sl_noPackage_tddl_fy_fg = new TreeItemData("分割", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                  new TreeItemData("分割", Icons.Filled.Label, "")
                }
            };
            //95
            var sl_noPackage_tddl_fy = new TreeItemData("分月", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-无套装-梯度达量-分月-分割
                    sl_noPackage_tddl_fy_fg,
                    //随量-无套装-梯度达量-分月-不分割
                    sl_noPackage_tddl_fy_bfg,
                }
            };
            //94
            var sl_noPackage_tddl = new TreeItemData("梯度达量", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-无套装-梯度达量-分月
                    sl_noPackage_tddl_fy,
                    //随量-无套装-梯度达量-累计
                    sl_noPackage_tddl_lj,
                }
            };
            #endregion

            #region //随量-无套装-分月达量
            //93
            var sl_noPackage_fydl_gd_xh = new TreeItemData("循环", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   new TreeItemData("循环", Icons.Filled.Label, "")
                }
            };
            //92
            var sl_noPackage_fydl_gd_dc = new TreeItemData("单次", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   new TreeItemData("单次", Icons.Filled.Label, "")
                }
            };
            //91
            var sl_noPackage_fydl_gd = new TreeItemData("固定", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-无套装-分月达量-固定-单次
                    sl_noPackage_fydl_gd_dc,
                    //随量-无套装-分月达量-固定-循环
                    sl_noPackage_fydl_gd_xh,
                }
            };
            //89
            var sl_noPackage_fydl_random_xh = new TreeItemData("循环", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   new TreeItemData("循环", Icons.Filled.Label, "")
                }
            };
            //88
            var sl_noPackage_fydl_random_dc = new TreeItemData("单次", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   new TreeItemData("单次", Icons.Filled.Label, "")
                }
            };
            //87
            var sl_noPackage_fydl_random = new TreeItemData("随量", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-无套装-分月达量-随量-单次
                    sl_noPackage_fydl_random_dc,
                    //随量-无套装-分月达量-随量-循环
                    sl_noPackage_fydl_random_xh,
                }
            };
            //86
            var sl_noPackage_fydl = new TreeItemData("分月达量", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-无套装-分月达量-随量
                    sl_noPackage_fydl_random,
                     //随量-无套装-分月达量-固定
                    sl_noPackage_fydl_gd,
                }
            };
            #endregion

            #region //随量-无套装-累计达量
            //85
            var sl_noPackage_ljdl_gd_xh = new TreeItemData("循环", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   new TreeItemData("循环", Icons.Filled.Label, "")
                }
            };
            //84
            var sl_noPackage_ljdl_gd_dc = new TreeItemData("单次", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   new TreeItemData("单次", Icons.Filled.Label, "")
                }
            };
            //83
            var sl_noPackage_ljdl_gd = new TreeItemData("固定", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-无套装-累计达量-固定-单次
                    sl_noPackage_ljdl_gd_dc,
                    //随量-无套装-累计达量-固定-循环
                    sl_noPackage_ljdl_gd_xh,
                }
            };
            //82
            var sl_noPackage_ljdl_random_xh = new TreeItemData("循环", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   new TreeItemData("循环", Icons.Filled.Label, "")
                }
            };
            //81
            var sl_noPackage_ljdl_random_dc = new TreeItemData("单次", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   new TreeItemData("单次", Icons.Filled.Label, "")
                }
            };
            //80
            var sl_noPackage_ljdl_random = new TreeItemData("随量", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-无套装-累计达量-随量-单次
                    sl_noPackage_ljdl_random_dc,
                    //随量-无套装-累计达量-随量-循环
                    sl_noPackage_ljdl_random_xh,
                }
            };
            //79
            var sl_noPackage_ljdl = new TreeItemData("累计达量", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-无套装-累计达量-随量
                    sl_noPackage_ljdl_random,
                     //随量-无套装-累计达量-固定
                    sl_noPackage_ljdl_gd,
                }
            };
            #endregion

            #region //随量-无套装-随量
            //78
            var sl_noPackage_random_fd = new TreeItemData("返点", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    new TreeItemData("随量-无套装-随量-返点", Icons.Filled.Label, "进店价*返点比例*销量")
                }
            };
            //77
            var sl_noPackage_random_gj = new TreeItemData("给酒", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    new TreeItemData("随量-无套装-随量-给酒", Icons.Filled.Label, "Σ【赠酒单价*赠酒量】*[销量/兑付基数]")
                }
            };
            //76
            var sl_noPackage_random_gq = new TreeItemData("给钱", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    new TreeItemData("随量-无套装-随量-给钱", Icons.Filled.Label, "单箱标准*产品销量（政策上的预估量或实际动销量）")
                }
            };
            //75
            var sl_noPackage_random = new TreeItemData("随量", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-无套装-随量-给钱
                    sl_noPackage_random_gq,
                    //随量-无套装-随量-给酒
                    sl_noPackage_random_gj,
                    //随量-无套装-随量-返点
                    sl_noPackage_random_fd,
                }
            };
            #endregion

            #region //随量-无套装
            //74
            var sl_noPackage = new TreeItemData("无套装", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-无套装-随量
                    sl_noPackage_random,
                    //随量-无套装-累计达量
                    sl_noPackage_ljdl,
                    //随量-无套装-分月达量
                    sl_noPackage_fydl,
                    //随量-无套装-梯度达量
                    sl_noPackage_tddl,
                }
            };
            #endregion


            #region //随量-有套装-有比例-按产品-梯度达量
            //73
            var sl_hasPackage_hasRatio_acp_tddl_lj_nofg = new TreeItemData("不分割", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    new TreeItemData("不分割", Icons.Filled.Label, "")
                }
            };
            //72
            var sl_hasPackage_hasRatio_acp_tddl_lj_fg = new TreeItemData("分割", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    new TreeItemData("分割", Icons.Filled.Label, "")
                }
            };
            //71
            var sl_hasPackage_hasRatio_acp_tddl_lj = new TreeItemData("累计", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-有比例-按产品-梯度达量-累计-分割
                    sl_hasPackage_hasRatio_acp_tddl_lj_fg,
                     //随量-有套装-有比例-按产品-梯度达量-累计-不分割
                    sl_hasPackage_hasRatio_acp_tddl_lj_nofg,
                }
            };
            //70
            var sl_hasPackage_hasRatio_acp_tddl_fy_nofg = new TreeItemData("不分割", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    new TreeItemData("不分割", Icons.Filled.Label, "")
                }
            };
            //69
            var sl_hasPackage_hasRatio_acp_tddl_fy_fg = new TreeItemData("分割", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    new TreeItemData("分割", Icons.Filled.Label, "")
                }
            };
            //68
            var sl_hasPackage_hasRatio_acp_tddl_fy = new TreeItemData("分月", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-有比例-按产品-梯度达量-分月-分割
                    sl_hasPackage_hasRatio_acp_tddl_fy_fg,
                     //随量-有套装-有比例-按产品-梯度达量-分月-不分割
                    sl_hasPackage_hasRatio_acp_tddl_fy_nofg,
                }
            };
            //67
            var sl_hasPackage_hasRatio_acp_tddl = new TreeItemData("梯度达量", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-有比例-按产品-梯度达量-分月
                    sl_hasPackage_hasRatio_acp_tddl_fy,
                      //随量-有套装-有比例-按产品-梯度达量-累计
                    sl_hasPackage_hasRatio_acp_tddl_lj
                }
            };
            #endregion

            #region //随量-有套装-有比例-按套装-分月达量
            //66
            var sl_hasPackage_hasRatio_acp_fydl_gd_xh = new TreeItemData("循环", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   new TreeItemData("循环", Icons.Filled.Label, "")
                }
            };
            //65
            var sl_hasPackage_hasRatio_acp_fydl_gd_dc = new TreeItemData("单次", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   new TreeItemData("单次", Icons.Filled.Label, "")
                }
            };
            //64 
            var sl_hasPackage_hasRatio_acp_fydl_gd = new TreeItemData("固定", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-有比例-按产品-分月达量-固定-单次
                    sl_hasPackage_hasRatio_acp_fydl_gd_dc,
                    //随量-有套装-有比例-按产品-分月达量-固定-循环
                    sl_hasPackage_hasRatio_acp_fydl_gd_xh,
                }
            };
            //63
            var sl_hasPackage_hasRatio_acp_fydl_random_xh = new TreeItemData("循环", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   new TreeItemData("循环", Icons.Filled.Label, "")
                }
            };
            //62
            var sl_hasPackage_hasRatio_acp_fydl_random_dc = new TreeItemData("单次", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   new TreeItemData("单次", Icons.Filled.Label, "")
                }
            };
            //61
            var sl_hasPackage_hasRatio_acp_fydl_random = new TreeItemData("随量", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-有比例-按产品-分月达量-随量-单次
                    sl_hasPackage_hasRatio_acp_fydl_random_dc,
                    //随量-有套装-有比例-按产品-分月达量-随量-循环
                    sl_hasPackage_hasRatio_acp_fydl_random_xh,
                }
            };
            //60
            var sl_hasPackage_hasRatio_acp_fydl = new TreeItemData("分月达量", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-有比例-按产品-分月达量-随量
                    sl_hasPackage_hasRatio_acp_fydl_random,
                      //随量-有套装-有比例-按产品-分月达量-固定
                    sl_hasPackage_hasRatio_acp_fydl_gd
                }
            };
            #endregion

            #region //随量-有套装-有比例-按产品-累计达量
            //59
            var sl_hasPackage_hasRatio_acp_ljdl_gd_xh = new TreeItemData("循环", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   new TreeItemData("循环", Icons.Filled.Label, "")
                }
            };
            //58
            var sl_hasPackage_hasRatio_acp_ljdl_gd_dc = new TreeItemData("单次", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   new TreeItemData("单次", Icons.Filled.Label, "")
                }
            };
            //57 
            var sl_hasPackage_hasRatio_acp_ljdl_gd = new TreeItemData("固定", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-有比例-按产品-累计达量-固定-单次
                    sl_hasPackage_hasRatio_acp_ljdl_gd_dc,
                    //随量-有套装-有比例-按产品-累计达量-固定-循环
                    sl_hasPackage_hasRatio_acp_ljdl_gd_xh,
                }
            };
            //56
            var sl_hasPackage_hasRatio_acp_ljdl_random_xh = new TreeItemData("循环", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   new TreeItemData("循环", Icons.Filled.Label, "")
                }
            };
            //55
            var sl_hasPackage_hasRatio_acp_ljdl_random_dc = new TreeItemData("单次", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   new TreeItemData("单次", Icons.Filled.Label, "")
                }
            };
            //54
            var sl_hasPackage_hasRatio_acp_ljdl_random = new TreeItemData("随量", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-有比例-按产品-累计达量-随量-单次
                    sl_hasPackage_hasRatio_acp_ljdl_random_dc,
                    //随量-有套装-有比例-按产品-累计达量-随量-循环
                    sl_hasPackage_hasRatio_acp_ljdl_random_xh,
                }
            };
            //53
            var sl_hasPackage_hasRatio_acp_ljdl = new TreeItemData("累计达量", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-有比例-按产品-累计达量-随量
                    sl_hasPackage_hasRatio_acp_ljdl_random,
                      //随量-有套装-有比例-按产品-累计达量-固定
                    sl_hasPackage_hasRatio_acp_ljdl_gd
                }
            };
            #endregion

            #region //随量-有套装-有比例-按产品-随量
            //52
            var sl_hasPackage_hasRatio_acp_random = new TreeItemData("随量", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-有比例-按产品-随量
                    new TreeItemData("套装数*投入产品标准", Icons.Filled.Label, "")
                }
            };
            #endregion

            #region //随量-有套装-有比例-按产品
            //51
            var sl_hasPackage_hasRatio_acp = new TreeItemData("按产品", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-有比例-按产品-随量
                    sl_hasPackage_hasRatio_acp_random,
                    //随量-有套装-有比例-按产品-累计达量
                    sl_hasPackage_hasRatio_acp_ljdl,
                     //随量-有套装-有比例-按产品-分月达量
                    sl_hasPackage_hasRatio_acp_fydl,
                     //随量-有套装-有比例-按产品-梯度达量
                    sl_hasPackage_hasRatio_acp_tddl,
                }
            };
            #endregion

            #region //随量-有套装-有比例-按套装-梯度达量
            //50
            var sl_hasPackage_hasRatio_atz_tddl_lj_nofg = new TreeItemData("不分割", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    new TreeItemData("不分割", Icons.Filled.Label, "")
                }
            };
            //49
            var sl_hasPackage_hasRatio_atz_tddl_lj_fg = new TreeItemData("分割", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    new TreeItemData("分割", Icons.Filled.Label, "")
                }
            };
            //48
            var sl_hasPackage_hasRatio_atz_tddl_lj = new TreeItemData("累计", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-有比例-按套装-梯度达量-累计-分割
                    sl_hasPackage_hasRatio_atz_tddl_lj_fg,
                     //随量-有套装-有比例-按套装-梯度达量-累计-不分割
                    sl_hasPackage_hasRatio_atz_tddl_lj_nofg,
                }
            };
            //47
            var sl_hasPackage_hasRatio_atz_tddl_fy_nofg = new TreeItemData("不分割", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    new TreeItemData("不分割", Icons.Filled.Label, "")
                }
            };
            //46
            var sl_hasPackage_hasRatio_atz_tddl_fy_fg = new TreeItemData("分割", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    new TreeItemData("分割", Icons.Filled.Label, "")
                }
            };
            //45
            var sl_hasPackage_hasRatio_atz_tddl_fy = new TreeItemData("分月", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-有比例-按套装-梯度达量-分月-分割
                    sl_hasPackage_hasRatio_atz_tddl_fy_fg,
                     //随量-有套装-有比例-按套装-梯度达量-分月-不分割
                    sl_hasPackage_hasRatio_atz_tddl_fy_nofg,
                }
            };
            //44
            var sl_hasPackage_hasRatio_atz_tddl = new TreeItemData("梯度达量", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-有比例-按套装-梯度达量-分月
                    sl_hasPackage_hasRatio_atz_tddl_fy,
                      //随量-有套装-有比例-按套装-梯度达量-累计
                    sl_hasPackage_hasRatio_atz_tddl_lj
                }
            };
            #endregion

            #region //随量-有套装-有比例-按套装-分月达量
            //43
            var sl_hasPackage_hasRatio_atz_fydl_gd_xh = new TreeItemData("循环", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   new TreeItemData("循环", Icons.Filled.Label, "")
                }
            };
            //42
            var sl_hasPackage_hasRatio_atz_fydl_gd_dc = new TreeItemData("单次", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   new TreeItemData("单次", Icons.Filled.Label, "")
                }
            };
            //41 
            var sl_hasPackage_hasRatio_atz_fydl_gd = new TreeItemData("固定", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-有比例-按套装-分月达量-固定-单次
                    sl_hasPackage_hasRatio_atz_fydl_gd_dc,
                    //随量-有套装-有比例-按套装-分月达量-固定-循环
                    sl_hasPackage_hasRatio_atz_fydl_gd_xh,
                }
            };
            //40
            var sl_hasPackage_hasRatio_atz_fydl_random_xh = new TreeItemData("循环", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   new TreeItemData("循环", Icons.Filled.Label, "")
                }
            };
            //39
            var sl_hasPackage_hasRatio_atz_fydl_random_dc = new TreeItemData("单次", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   new TreeItemData("单次", Icons.Filled.Label, "")
                }
            };
            //38
            var sl_hasPackage_hasRatio_atz_fydl_random = new TreeItemData("随量", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-有比例-按套装-分月达量-随量-单次
                    sl_hasPackage_hasRatio_atz_fydl_random_dc,
                    //随量-有套装-有比例-按套装-分月达量-随量-循环
                    sl_hasPackage_hasRatio_atz_fydl_random_xh,
                }
            };
            //37
            var sl_hasPackage_hasRatio_atz_fydl = new TreeItemData("分月达量", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-有比例-按套装-分月达量-随量
                    sl_hasPackage_hasRatio_atz_fydl_random,
                      //随量-有套装-有比例-按套装-分月达量-固定
                    sl_hasPackage_hasRatio_atz_fydl_gd
                }
            };
            #endregion

            #region //随量-有套装-有比例-按套装-累计达量
            //43
            var sl_hasPackage_hasRatio_atz_ljdl_gd_xh = new TreeItemData("循环", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   new TreeItemData("循环", Icons.Filled.Label, "")
                }
            };
            //42
            var sl_hasPackage_hasRatio_atz_ljdl_gd_dc = new TreeItemData("单次", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   new TreeItemData("单次", Icons.Filled.Label, "")
                }
            };
            //41 
            var sl_hasPackage_hasRatio_atz_ljdl_gd = new TreeItemData("固定", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-有比例-按套装-累计达量-固定-单次
                    sl_hasPackage_hasRatio_atz_ljdl_gd_dc,
                    //随量-有套装-有比例-按套装-累计达量-固定-循环
                    sl_hasPackage_hasRatio_atz_ljdl_gd_xh,
                }
            };
            //40
            var sl_hasPackage_hasRatio_atz_ljdl_random_xh = new TreeItemData("循环", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   new TreeItemData("循环", Icons.Filled.Label, "")
                }
            };
            //39
            var sl_hasPackage_hasRatio_atz_ljdl_random_dc = new TreeItemData("单次", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   new TreeItemData("单次", Icons.Filled.Label, "")
                }
            };
            //38
            var sl_hasPackage_hasRatio_atz_ljdl_random = new TreeItemData("随量", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-有比例-按套装-累计达量-随量-单次
                    sl_hasPackage_hasRatio_atz_ljdl_random_dc,
                    //随量-有套装-有比例-按套装-累计达量-随量-循环
                    sl_hasPackage_hasRatio_atz_ljdl_random_xh,
                }
            };
            //37
            var sl_hasPackage_hasRatio_atz_ljdl = new TreeItemData("累计达量", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-有比例-按套装-累计达量-随量
                    sl_hasPackage_hasRatio_atz_ljdl_random,
                      //随量-有套装-有比例-按套装-累计达量-固定
                    sl_hasPackage_hasRatio_atz_ljdl_gd
                }
            };
            #endregion

            #region //随量-有套装-有比例-按套装-随量
            //36
            var sl_hasPackage_hasRatio_atz_random = new TreeItemData("随量", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-有比例-按套装-随量
                    new TreeItemData("随量-有套装-有比例-按套装-随量", Icons.Filled.Label, "套装数*套装标准")
                }
            };
            #endregion

            #region //随量-有套装-有比例-按套装
            //35
            var sl_hasPackage_hasRatio_atz = new TreeItemData("按套装", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-有比例-按套装-随量
                    sl_hasPackage_hasRatio_atz_random,
                    //随量-有套装-有比例-按套装-累计达量
                    sl_hasPackage_hasRatio_atz_ljdl,
                     //随量-有套装-有比例-按套装-分月达量
                    sl_hasPackage_hasRatio_atz_fydl,
                     //随量-有套装-有比例-按套装-梯度达量
                    sl_hasPackage_hasRatio_atz_tddl,
                }
            };
            #endregion

            #region //随量-有套装-有比例
            //34
            var sl_hasPackage_hasRatio = new TreeItemData("有比例", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-有比例-按套装
                    sl_hasPackage_hasRatio_atz,
                    //随量-有套装-有比例-按产品
                    sl_hasPackage_hasRatio_acp,
                }
            };
            #endregion


            #region //随量-有套装-无比例-不保量保结构-累计达量
            //33
            var sl_hasPackage_noRatio_noblbjg_ljdl_gd_xh = new TreeItemData("循环", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                     //随量-有套装-无比例-不保量保结构-累计达量-固定-循环-按产品
                    new TreeItemData("按产品", Icons.Filled.Label, "")
                }
            };
            //32
            var sl_hasPackage_noRatio_noblbjg_ljdl_gd_dc = new TreeItemData("单次", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-无比例-不保量保结构-累计达量-固定-单次-按产品
                     new TreeItemData("按产品", Icons.Filled.Label, "")
                }
            };
            //31
            var sl_hasPackage_noRatio_noblbjg_ljdl_gd = new TreeItemData("固定", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   //随量-有套装-无比例-不保量保结构-累计达量-固定-单次
                   sl_hasPackage_noRatio_noblbjg_ljdl_gd_dc,
                   //随量-有套装-无比例-不保量保结构-累计达量-固定-循环
                   sl_hasPackage_noRatio_noblbjg_ljdl_gd_xh,
                }
            };
            //30-1
            var sl_hasPackage_noRatio_noblbjg_ljdl_sl_xh = new TreeItemData("循环", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                     //随量-有套装-无比例-不保量保结构-累计达量-随量-循环-按产品
                     new TreeItemData("按产品", Icons.Filled.Label, "")
                }
            };
            //29-1
            var sl_hasPackage_noRatio_noblbjg_ljdl_sl_dc = new TreeItemData("单次", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                     //随量-有套装-无比例-不保量保结构-累计达量-随量-单次-按产品
                     new TreeItemData("按产品", Icons.Filled.Label, ""),
                }
            };
            //28
            var sl_hasPackage_noRatio_noblbjg_ljdl_sl = new TreeItemData("随量", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   //随量-有套装-无比例-不保量保结构-累计达量-随量-单次
                   sl_hasPackage_noRatio_noblbjg_ljdl_sl_dc,
                   //随量-有套装-无比例-不保量保结构-累计达量-随量-循环
                   sl_hasPackage_noRatio_noblbjg_ljdl_sl_xh,
                }
            };
            //27
            var sl_hasPackage_noRatio_noblbjg_ljdl = new TreeItemData("累计达量", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                  //随量-有套装-无比例-不保量保结构-累计达量-随量
                   sl_hasPackage_noRatio_noblbjg_ljdl_sl,
                   //随量-有套装-无比例-不保量保结构-累计达量-固定
                   sl_hasPackage_noRatio_noblbjg_ljdl_gd,
                }
            };
            #endregion

            #region //随量-有套装-无比例-不保量保结构-分月达量
            //26
            var sl_hasPackage_noRatio_noblbjg_fydl_gd_xh = new TreeItemData("循环", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                     //随量-有套装-无比例-不保量保结构-分月达量-固定-循环-按产品
                    new TreeItemData("按产品", Icons.Filled.Label, "")
                }
            };
            //25
            var sl_hasPackage_noRatio_noblbjg_fydl_gd_dc = new TreeItemData("单次", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-无比例-不保量保结构-分月达量-固定-单次-按产品
                     new TreeItemData("按产品", Icons.Filled.Label, "")
                }
            };
            //24
            var sl_hasPackage_noRatio_noblbjg_fydl_gd = new TreeItemData("固定", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   //随量-有套装-无比例-不保量保结构-分月达量-固定-单次
                   sl_hasPackage_noRatio_noblbjg_fydl_gd_dc,
                   //随量-有套装-无比例-不保量保结构-分月达量-固定-循环
                   sl_hasPackage_noRatio_noblbjg_fydl_gd_xh,
                }
            };
            //23-1
            var sl_hasPackage_noRatio_noblbjg_fydl_sl_xh = new TreeItemData("循环", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                     //随量-有套装-无比例-不保量保结构-分月达量-随量-循环-按产品
                     new TreeItemData("按产品", Icons.Filled.Label, "")
                }
            };
            //22-1
            var sl_hasPackage_noRatio_noblbjg_fydl_sl_dc = new TreeItemData("单次", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                     //随量-有套装-无比例-不保量保结构-分月达量-随量-单次-按产品
                     new TreeItemData("按产品", Icons.Filled.Label, ""),
                }
            };
            //21
            var sl_hasPackage_noRatio_noblbjg_fydl_sl = new TreeItemData("随量", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   //随量-有套装-无比例-不保量保结构-分月达量-随量-单次
                   sl_hasPackage_noRatio_noblbjg_fydl_sl_dc,
                   //随量-有套装-无比例-不保量保结构-分月达量-随量-循环
                   sl_hasPackage_noRatio_noblbjg_fydl_sl_xh,
                }
            };
            //20
            var sl_hasPackage_noRatio_noblbjg_fydl = new TreeItemData("分月达量", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                  //随量-有套装-无比例-不保量保结构-分月达量-随量
                   sl_hasPackage_noRatio_noblbjg_fydl_sl,
                   //随量-有套装-无比例-不保量保结构-分月达量-固定
                   sl_hasPackage_noRatio_noblbjg_fydl_gd,
                }
            };
            #endregion

            #region //随量-有套装-无比例-不保量保结构
            //19
            var sl_hasPackage_noRatio_noblbjg = new TreeItemData("不保量保结构", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-无比例-不保量保结构-分月达量
                    sl_hasPackage_noRatio_noblbjg_fydl,
                    //随量-有套装-无比例-不保量保结构-累计达量
                    sl_hasPackage_noRatio_noblbjg_ljdl,
                }
            };
            #endregion

            #region  //随量-有套装-无比例-保量保结构-累计达量
            //18
            var sl_hasPackage_noRatio_blbjg_ljdl_gd_xh = new TreeItemData("循环", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                     //随量-有套装-无比例-保量保结构-累计达量-固定-循环-按产品
                    new TreeItemData("按产品", Icons.Filled.Label, "")
                }
            };
            //17
            var sl_hasPackage_noRatio_blbjg_ljdl_gd_dc = new TreeItemData("单次", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-无比例-保量保结构-累计达量-固定-单次-按产品
                     new TreeItemData("按产品", Icons.Filled.Label, "")
                }
            };
            //16
            var sl_hasPackage_noRatio_blbjg_ljdl_gd = new TreeItemData("固定", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   //随量-有套装-无比例-保量保结构-累计达量-固定-单次
                   sl_hasPackage_noRatio_blbjg_ljdl_gd_dc,
                   //随量-有套装-无比例-保量保结构-累计达量-固定-循环
                   sl_hasPackage_noRatio_blbjg_ljdl_gd_xh,
                }
            };
            //15-1
            var sl_hasPackage_noRatio_blbjg_ljdl_sl_xh = new TreeItemData("循环", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                     //随量-有套装-无比例-保量保结构-累计达量-随量-循环-按产品
                     new TreeItemData("按产品", Icons.Filled.Label, "")
                }
            };
            //14-1
            var sl_hasPackage_noRatio_blbjg_ljdl_sl_dc = new TreeItemData("单次", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                     //随量-有套装-无比例-保量保结构-累计达量-随量-单次-按产品
                     new TreeItemData("按产品", Icons.Filled.Label, ""),
                }
            };
            //13
            var sl_hasPackage_noRatio_blbjg_ljdl_sl = new TreeItemData("随量", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   //随量-有套装-无比例-保量保结构-累计达量-随量-单次
                   sl_hasPackage_noRatio_blbjg_ljdl_sl_dc,
                   //随量-有套装-无比例-保量保结构-累计达量-随量-循环
                   sl_hasPackage_noRatio_blbjg_ljdl_sl_xh,
                }
            };
            //12
            var sl_hasPackage_noRatio_blbjg_ljdl = new TreeItemData("累计达量", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                  //随量-有套装-无比例-保量保结构-累计达量-随量
                   sl_hasPackage_noRatio_blbjg_ljdl_sl,
                   //随量-有套装-无比例-保量保结构-累计达量-固定
                   sl_hasPackage_noRatio_blbjg_ljdl_gd,
                }
            };
            #endregion

            #region //随量-有套装-无比例-保量保结构-分月达量
            //11
            var sl_hasPackage_noRatio_blbjg_fydl_gd_xh = new TreeItemData("循环", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                     //随量-有套装-无比例-保量保结构-分月达量-固定-循环-按产品
                    new TreeItemData("按产品", Icons.Filled.Label, "")
                }
            };
            //10
            var sl_hasPackage_noRatio_blbjg_fydl_gd_dc = new TreeItemData("单次", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-无比例-保量保结构-分月达量-固定-单次-按产品
                     new TreeItemData("按产品", Icons.Filled.Label, "")
                }
            };
            //9
            var sl_hasPackage_noRatio_blbjg_fydl_gd = new TreeItemData("固定", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   //随量-有套装-无比例-保量保结构-分月达量-固定-单次
                   sl_hasPackage_noRatio_blbjg_fydl_gd_dc,
                   //随量-有套装-无比例-保量保结构-分月达量-固定-循环
                   sl_hasPackage_noRatio_blbjg_fydl_gd_xh,
                }
            };
            //8-1
            var sl_hasPackage_noRatio_blbjg_fydl_sl_xh = new TreeItemData("循环", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                     //随量-有套装-无比例-保量保结构-分月达量-随量-循环-按产品
                     new TreeItemData("按产品", Icons.Filled.Label, "")
                }
            };
            //7-1
            var sl_hasPackage_noRatio_blbjg_fydl_sl_dc = new TreeItemData("单次", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                     //随量-有套装-无比例-保量保结构-分月达量-随量-单次-按产品
                     new TreeItemData("按产品", Icons.Filled.Label, ""),
                }
            };
            //6
            var sl_hasPackage_noRatio_blbjg_fydl_sl = new TreeItemData("随量", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   //随量-有套装-无比例-保量保结构-分月达量-随量-单次
                   sl_hasPackage_noRatio_blbjg_fydl_sl_dc,
                   //随量-有套装-无比例-保量保结构-分月达量-随量-循环
                   sl_hasPackage_noRatio_blbjg_fydl_sl_xh,
                }
            };
            //5
            var sl_hasPackage_noRatio_blbjg_fydl = new TreeItemData("分月达量", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                  //随量-有套装-无比例-保量保结构-分月达量-随量
                   sl_hasPackage_noRatio_blbjg_fydl_sl,
                   //随量-有套装-无比例-保量保结构-分月达量-固定
                   sl_hasPackage_noRatio_blbjg_fydl_gd,
                }
            };
            #endregion

            #region //随量-有套装-无比例-保量保结构
            //4
            var sl_hasPackage_noRatio_blbjg = new TreeItemData("保量保结构", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-无比例-保量保结构-分月达量
                    sl_hasPackage_noRatio_blbjg_fydl,
                    //随量-有套装-无比例-保量保结构-累计达量
                    sl_hasPackage_noRatio_blbjg_ljdl,
                }
            };
            #endregion

            #region //随量-有套装-无比例
            //3
            var sl_hasPackage_noRatio = new TreeItemData("无比例", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                     //随量-有套装-无比例-保量保结构
                    sl_hasPackage_noRatio_blbjg,
                     //随量-有套装-无比例-不保量保结构
                    sl_hasPackage_noRatio_noblbjg,
                }
            };
            #endregion

            #region //随量-有套装
            //2
            var sl_hasPackage = new TreeItemData("有套装", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //随量-有套装-有比例
                    sl_hasPackage_hasRatio,
                    //随量-有套装-无比例
                    sl_hasPackage_noRatio,
                }
            };
            #endregion

            #region //随量

            //1
            TreeItems.Add(new TreeItemData("随量", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                   //随量-有套装
                   sl_hasPackage,
                   //随量-无套装
                   sl_noPackage,
                }
            });

            #endregion

            #region //固定

            //0
            TreeItems.Add(new TreeItemData("固定", Icons.Filled.Label)
            {
                IsExpanded = true,
                TreeItems = new HashSet<TreeItemData>()
                {
                    //固定-固定给钱
                    gd_gdgq,
                    //固定-固定给酒
                    gd_gdgj
                }
            });

            #endregion

        }

        private void CalcFeeCount()
        {
        }

    }
}
