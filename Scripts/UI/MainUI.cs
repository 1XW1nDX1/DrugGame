using Godot;
using FrontlinePharma.Core;

namespace FrontlinePharma.UI
{
    /// <summary>
    /// 主界面控制器。
    /// 负责：
    /// 1. 左侧四个按钮切换中央面板
    /// 2. 刷新顶部天数显示
    /// 3. 刷新数值展示面板
    /// </summary>
    public partial class MainUI : Node
    {
        /// <summary>
        /// 研发面板。
        /// </summary>
        [Export]
        public Control ResearchPanel { get; set; }

        /// <summary>
        /// 审批面板。
        /// </summary>
        [Export]
        public Control ApprovalPanel { get; set; }

        /// <summary>
        /// 生产面板。
        /// </summary>
        [Export]
        public Control ProductionPanel { get; set; }

        /// <summary>
        /// 数值展示面板。
        /// </summary>
        [Export]
        public Control StatsPanel { get; set; }

        /// <summary>
        /// 顶部显示当前天数的标签。
        /// </summary>
        [Export]
        public Label DayLabel { get; set; }

        /// <summary>
        /// 数值展示面板中的文本标签。
        /// </summary>
        [Export]
        public Label StatsLabel { get; set; }

        public override void _Ready()
        {
            GameManager.Instance.StateChanged += RefreshUI;
            ShowResearchPanel();
            RefreshUI();
        }

        public override void _ExitTree()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StateChanged -= RefreshUI;
            }
        }

        /// <summary>
        /// 显示研发面板，隐藏其他面板。
        /// 常用于左侧“研发药剂”按钮的 Pressed 信号绑定。
        /// </summary>
        /// <returns>无返回值。</returns>
        public void ShowResearchPanel()
        {
            ShowOnly(ResearchPanel);
        }

        /// <summary>
        /// 显示审批面板，隐藏其他面板。
        /// 常用于左侧“审批流程”按钮的 Pressed 信号绑定。
        /// </summary>
        /// <returns>无返回值。</returns>
        public void ShowApprovalPanel()
        {
            ShowOnly(ApprovalPanel);
        }

        /// <summary>
        /// 显示生产面板，隐藏其他面板。
        /// 常用于左侧“生产流程”按钮的 Pressed 信号绑定。
        /// </summary>
        /// <returns>无返回值。</returns>
        public void ShowProductionPanel()
        {
            ShowOnly(ProductionPanel);
        }

        /// <summary>
        /// 显示数值展示面板，隐藏其他面板。
        /// 常用于左侧“数值展示”按钮的 Pressed 信号绑定。
        /// </summary>
        /// <returns>无返回值。</returns>
        public void ShowStatsPanel()
        {
            ShowOnly(StatsPanel);
            RefreshUI();
        }

        /// <summary>
        /// 根据当前 GameState 刷新界面上的文本。
        /// 包括顶部天数和数值展示面板中的核心数值。
        /// </summary>
        /// <returns>无返回值。</returns>
        public void RefreshUI()
        {
            if (DayLabel != null)
            {
                DayLabel.Text = $"Day {GameManager.Instance.State.Day}";
            }

            if (StatsLabel != null)
            {
                var state = GameManager.Instance.State;
                StatsLabel.Text =
                    $"当前天数：{state.Day}\n" +
                    $"研发进度：{state.ResearchProgress}\n" +
                    $"审批进度：{state.ApprovalProgress}\n" +
                    $"生产进度：{state.ProductionProgress}\n" +
                    $"前线需求：{state.FrontlineDemand}";
            }
        }

        /// <summary>
        /// 只显示目标面板，并隐藏其他三个面板。
        /// </summary>
        /// <param name="target">要显示的目标面板。</param>
        /// <returns>无返回值。</returns>
        private void ShowOnly(Control target)
        {
            if (ResearchPanel != null) ResearchPanel.Visible = false;
            if (ApprovalPanel != null) ApprovalPanel.Visible = false;
            if (ProductionPanel != null) ProductionPanel.Visible = false;
            if (StatsPanel != null) StatsPanel.Visible = false;

            if (target != null)
            {
                target.Visible = true;
            }
        }
    }
}