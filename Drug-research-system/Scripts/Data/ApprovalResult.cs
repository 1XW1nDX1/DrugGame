/// <summary>
/// 审批结果
/// </summary>
public class ApprovalResult
{
	public bool IsApproved;        // MVP 永远 true
	public float TestCompletion;   // 测试完成度 0~100
	public float FinalHiddenRisk;  // 修正后的隐藏风险
	public string RiskGrade;       // "低" / "中" / "高"
	public string ApprovalNote;    // 提示文本
}