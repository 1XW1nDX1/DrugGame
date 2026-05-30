/// <summary>
/// 成分定义 — 硬编码数据类
/// </summary>
public class IngredientDef
{
	public string Id;
	public string Category;   // "Base" / "Active" / "Stabilizer"
	public string DisplayName; // 中文显示名

	// 数值修正
	public float EfficacyMod;
	public float SpeedMod;
	public float StabilityMod;
	public float SideEffectMod;
	public float CostMod;
}