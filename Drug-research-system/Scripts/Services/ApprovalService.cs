/// <summary>
/// 审批服务 — 三种审批方式，影响隐藏风险和测试完成度
/// </summary>
public static class ApprovalService
{
	public enum Mode
	{
		FullTest,       // 完整测试：3回合，风险-85%
		SimplifiedTest, // 简化测试：1回合，风险-40%
		Emergency       // 紧急授权：0回合，风险不变
	}

	public static ApprovalResult Approve(DrugPrototype prototype, Mode mode)
	{
		float testCompletion;
		float riskMultiplier;

		switch (mode)
		{
			case Mode.FullTest:
				testCompletion = 90f;
				riskMultiplier = 0.15f; // 降低85%
				break;
			case Mode.SimplifiedTest:
				testCompletion = 50f;
				riskMultiplier = 0.60f; // 降低40%
				break;
			case Mode.Emergency:
			default:
				testCompletion = 10f;
				riskMultiplier = 1.00f; // 不变
				break;
		}

		float finalRisk = prototype.HiddenRisk * riskMultiplier;

		string riskGrade = finalRisk switch
		{
			<= 33f => "低",
			<= 66f => "中",
			_ => "高"
		};

		return new ApprovalResult
		{
			IsApproved = true,
			TestCompletion = testCompletion,
			FinalHiddenRisk = finalRisk,
			RiskGrade = riskGrade,
			ApprovalNote = riskGrade == "高"
				? "警告：隐藏风险较高，建议增加稳定成分或选择更严格测试"
				: "审批通过，可提交生产"
		};
	}
}