using Godot;
using System.Threading.Tasks;
using FrontlinePharma.Core;

namespace FrontlinePharma.UI
{
    /// <summary>
    /// “下一天”按钮控制器。
    /// 负责：
    /// 1. 防止重复点击
    /// 2. 黑屏淡出 / 淡入
    /// 3. 调用 TurnManager 推进下一天
    /// </summary>
    public partial class NextDayController : Control
    {
        /// <summary>
        /// 黑屏遮罩层。
        /// 必须是 ColorRect，且覆盖全屏。
        /// </summary>
        [Export]
        public ColorRect FadeOverlay { get; set; }

        /// <summary>
        /// “下一天”按钮。
        /// 转场期间会被禁用。
        /// </summary>
        [Export]
        public Button NextDayButton { get; set; }

        /// <summary>
        /// 回合管理器。
        /// 调用其 ProcessNextDay() 执行每日推进。
        /// </summary>
        [Export]
        public TurnManager TurnManager { get; set; }

        /// <summary>
        /// 黑屏淡出或淡入动画时长，单位秒。
        /// </summary>
        [Export]
        public float FadeDuration { get; set; } = 0.4f;

        public override void _Ready()
        {
            if (FadeOverlay != null)
            {
                var color = FadeOverlay.Color;
                color.A = 0f;
                FadeOverlay.Color = color;
            }
        }

        /// <summary>
        /// 处理“下一天”按钮点击事件。
        /// 使用方式：
        /// 1. 在编辑器中把按钮 Pressed 信号绑到本方法
        /// 2. 或由代码直接调用
        /// 
        /// 执行流程：
        /// - 判断当前是否正在转场
        /// - 黑屏淡入
        /// - 推进一天
        /// - 黑屏淡出
        /// 
        /// 返回值：
        /// - Task，异步方法。一般由 Godot 信号直接调用时无需手动 await。
        /// </summary>
        /// <returns>异步任务。</returns>
        public async Task OnNextDayButtonPressed()
        {
            if (GameManager.Instance.IsTransitioning)
            {
                return;
            }

            GameManager.Instance.IsTransitioning = true;

            if (NextDayButton != null)
            {
                NextDayButton.Disabled = true;
            }

            await FadeTo(1.0f, FadeDuration);

            TurnManager?.ProcessNextDay();

            await FadeTo(0.0f, FadeDuration);

            if (NextDayButton != null)
            {
                NextDayButton.Disabled = false;
            }

            GameManager.Instance.IsTransitioning = false;
        }

        /// <summary>
        /// 控制 FadeOverlay 的透明度渐变。
        /// </summary>
        /// <param name="targetAlpha">目标透明度，范围 0 到 1。</param>
        /// <param name="duration">渐变时长，单位秒。</param>
        /// <returns>异步任务，Tween 完成后结束。</returns>
        private async Task FadeTo(float targetAlpha, float duration)
        {
            if (FadeOverlay == null)
            {
                return;
            }

            var tween = CreateTween();
            tween.TweenProperty(FadeOverlay, "color:a", targetAlpha, duration);
            await ToSignal(tween, Tween.SignalName.Finished);
        }
    }
}