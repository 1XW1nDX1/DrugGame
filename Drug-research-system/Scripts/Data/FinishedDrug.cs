/// <summary>
/// 药品成品 — 研发+审批的最终输出，交给制造系统和主系统
/// </summary>
public class FinishedDrug
{
	// 身份标识
	public string DrugId;
	public string DrugName;
	public string DrugFamily;

	// 审批信息
	public string ApprovalMode;
	public float TestCompletion;
	public string RiskGrade;

	// 最终药效参数 (0~100)
	public float Efficacy;
	public float OnsetSpeed;
	public float Duration;
	public float Stability;
	public float SideEffect;

	// 生产相关
	public float BaseCost;
	public string[] RequiredMaterials;

	// 风险标记
	public float HiddenRisk;
}