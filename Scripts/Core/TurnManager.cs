using Godot;
using System;

namespace FrontlinePharma.Core
{
    /// <summary>
    /// 回合流程管理器。
    /// 当前版本职责较轻，主要作为“按天推进逻辑”的独立入口。
    /// 后续可扩展为：
    /// - 每日事件结算
    /// - 战报刷新
    /// - 特殊随机事件
    /// - 章节推进
    /// </summary>
    public partial class TurnManager : Node
    {
        /// <summary>
        /// 执行一次下一天逻辑。
        /// 当前实现内部调用 GameManager.AdvanceDay()。
        /// </summary>
        /// <returns>无返回值。</returns>
        public void ProcessNextDay()
        {
            GameManager.Instance.AdvanceDay();
        }
    }
}