using Godot;
using FrontlinePharma.Core;

namespace FrontlinePharma.UI
{
    /// <summary>
    /// 功能面板脚本。
    /// 用于研发、审批、生产三个面板的共用逻辑。
    /// 点击按钮后，根据 PanelType 修改对应进度。
    /// </summary>
    public partial class ActionPanel : PanelContainer
    {
        /// <summary>
        /// 面板类型。
        /// 可选值：
        /// Research / Approval / Production
        /// 该值决定当前面板按钮点击后增加哪一类进度。
        /// </summary>
        [Export]
        public string PanelType { get; set; } = "";

        /// <summary>
        /// 每次点击操作按钮时增加的进度值。
        /// 默认值为 10。
        /// </summary>
        [Export]
        public int ProgressPerClick { get; set; } = 10;

        /// <summary>
        /// 当前面板中的操作按钮节点。
        /// 需要在编辑器中拖拽绑定。
        /// </summary>
        [Export]
        public Button ActionButton { get; set; }

        public override void _Ready()
        {
            if (ActionButton != null)
            {
                ActionButton.Pressed += OnActionButtonPressed;
            }
        }

        /// <summary>
        /// 处理操作按钮点击事件。
        /// 根据 PanelType 调用不同的 GameManager 接口。
        /// </summary>
        /// <returns>无返回值。</returns>
        public void OnActionButtonPressed()
        {
            switch (PanelType)
            {
                case "Research":
                    GameManager.Instance.AddResearchProgress(ProgressPerClick);
                    break;

                case "Approval":
                    GameManager.Instance.AddApprovalProgress(ProgressPerClick);
                    break;

                case "Production":
                    GameManager.Instance.AddProductionProgress(ProgressPerClick);
                    break;

                default:
                    GD.PrintErr($"Unknown PanelType: {PanelType}");
                    break;
            }
        }
    }
}