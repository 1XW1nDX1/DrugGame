using Godot;
using System;
using FrontlinePharma.Core;

namespace FrontlinePharma.Core
{
    /// <summary>
    /// 游戏全局管理器。
    /// 负责持有 GameState，提供外部修改数据的统一接口。
    /// 不直接负责界面逻辑，但会通过事件通知界面刷新。
    /// </summary>
    public partial class GameManager : Node
    {
        /// <summary>
        /// 全局单例。
        /// 方便其他脚本通过 GameManager.Instance 访问当前游戏状态。
        /// </summary>
        public static GameManager Instance { get; private set; }

        /// <summary>
        /// 当前游戏状态对象。
        /// 其他类应优先通过本管理器暴露的方法修改状态，而不是直接写字段。
        /// </summary>
        public GameState State { get; private set; } = new();

        /// <summary>
        /// 当前是否处于下一天的转场中。
        /// true 表示黑屏过场正在执行，此时应禁止重复点击“下一天”。
        /// </summary>
        public bool IsTransitioning { get; set; } = false;

        /// <summary>
        /// 当游戏数据发生变化时触发。
        /// UI 层可订阅该事件，用于刷新显示。
        /// </summary>
        public event Action StateChanged;

        public override void _Ready()
        {
            Instance = this;
        }

        /// <summary>
        /// 增加研发进度。
        /// </summary>
        /// <param name="value">要增加的数值，推荐正整数。</param>
        /// <returns>无返回值。</returns>
        public void AddResearchProgress(int value)
        {
            State.ResearchProgress += value;
            NotifyStateChanged();
        }

        /// <summary>
        /// 增加审批进度。
        /// </summary>
        /// <param name="value">要增加的数值，推荐正整数。</param>
        /// <returns>无返回值。</returns>
        public void AddApprovalProgress(int value)
        {
            State.ApprovalProgress += value;
            NotifyStateChanged();
        }

        /// <summary>
        /// 增加生产进度。
        /// </summary>
        /// <param name="value">要增加的数值，推荐正整数。</param>
        /// <returns>无返回值。</returns>
        public void AddProductionProgress(int value)
        {
            State.ProductionProgress += value;
            NotifyStateChanged();
        }

        /// <summary>
        /// 推进到下一天，并执行一次简化版结算。
        /// 结算规则：
        /// 1. 天数 +1
        /// 2. 前线需求默认 +5
        /// 3. 若研发、审批、生产三项进度都 >= 20，则额外使前线需求 -10
        /// 4. 前线需求不会低于 0
        /// </summary>
        /// <returns>无返回值。</returns>
        public void AdvanceDay()
        {
            State.Day += 1;
            State.FrontlineDemand += 5;

            if (State.ResearchProgress >= 20 &&
                State.ApprovalProgress >= 20 &&
                State.ProductionProgress >= 20)
            {
                State.FrontlineDemand -= 10;
            }

            State.FrontlineDemand = Math.Max(0, State.FrontlineDemand);

            NotifyStateChanged();
        }

        /// <summary>
        /// 重置整局数据到初始状态。
        /// </summary>
        /// <returns>无返回值。</returns>
        public void ResetGame()
        {
            State.Reset();
            NotifyStateChanged();
        }

        /// <summary>
        /// 触发 StateChanged 事件，通知界面层刷新。
        /// </summary>
        /// <returns>无返回值。</returns>
        public void NotifyStateChanged()
        {
            StateChanged?.Invoke();
        }
    }
}