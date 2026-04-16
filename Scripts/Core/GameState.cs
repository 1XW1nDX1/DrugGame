using Godot;
using System;

namespace FrontlinePharma.Core
{
    /// <summary>
    /// 游戏运行时数据模型。
    /// 该类只负责存储当前局内的核心数值，不负责界面更新，也不负责流程控制。
    /// </summary>
    public class GameState
    {
        /// <summary>
        /// 当前天数。
        /// 初始值为 1。
        /// </summary>
        public int Day { get; set; } = 1;

        /// <summary>
        /// 研发进度。
        /// 通过研发面板中的操作增加。
        /// </summary>
        public int ResearchProgress { get; set; } = 0;

        /// <summary>
        /// 审批进度。
        /// 通过审批面板中的操作增加。
        /// </summary>
        public int ApprovalProgress { get; set; } = 0;

        /// <summary>
        /// 生产进度。
        /// 通过生产面板中的操作增加。
        /// </summary>
        public int ProductionProgress { get; set; } = 0;

        /// <summary>
        /// 前线需求值。
        /// 每过一天默认增加，若三项进度足够则会在每日结算时下降。
        /// </summary>
        public int FrontlineDemand { get; set; } = 100;

        /// <summary>
        /// 将所有数据重置为默认值。
        /// </summary>
        public void Reset()
        {
            Day = 1;
            ResearchProgress = 0;
            ApprovalProgress = 0;
            ProductionProgress = 0;
            FrontlineDemand = 100;
        }
    }
}