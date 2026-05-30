using System.Collections.Generic;

/// <summary>
/// 硬编码成分与工艺数据库 — MVP 止血路线专用
/// </summary>
public static class IngredientDB
{
	private static readonly Dictionary<string, IngredientDef> _ingredients = new();
	private static readonly Dictionary<string, ProcessDef> _processes = new();

	static IngredientDB()
	{
		// ── 基础成分（1 个）──
		AddIngredient(new IngredientDef
		{
			Id = "HemostasisBase",
			Category = "Base",
			DisplayName = "止血基底",
			EfficacyMod = 0, SpeedMod = 0, StabilityMod = 0, SideEffectMod = 0, CostMod = 0
		});

		// ── 活性成分（1 个）──
		AddIngredient(new IngredientDef
		{
			Id = "RapidClot",
			Category = "Active",
			DisplayName = "速凝提取物",
			EfficacyMod = 30, SpeedMod = 20, StabilityMod = 0, SideEffectMod = 15, CostMod = 0
		});

		// ── 稳定成分（2 个）──
		AddIngredient(new IngredientDef
		{
			Id = "MildStabilizer",
			Category = "Stabilizer",
			DisplayName = "温和稳定剂",
			EfficacyMod = 0, SpeedMod = -5, StabilityMod = 20, SideEffectMod = -15, CostMod = 0
		});
		AddIngredient(new IngredientDef
		{
			Id = "BufferAgent",
			Category = "Stabilizer",
			DisplayName = "缓冲剂",
			EfficacyMod = -5, SpeedMod = 0, StabilityMod = 10, SideEffectMod = -10, CostMod = 0
		});

		// ── 工艺（2 个）──
		AddProcess(new ProcessDef
		{
			Id = "StandardFilter",
			DisplayName = "标准过滤",
			SpeedMod = 0, StabilityMod = 15, SideEffectMod = -5, CostMod = 10
		});
		AddProcess(new ProcessDef
		{
			Id = "RapidMix",
			DisplayName = "快速混合",
			SpeedMod = 15, StabilityMod = -15, SideEffectMod = 10, CostMod = 0
		});
	}

	private static void AddIngredient(IngredientDef def) => _ingredients[def.Id] = def;
	private static void AddProcess(ProcessDef def) => _processes[def.Id] = def;

	public static IngredientDef GetIngredient(string id) =>
		_ingredients.TryGetValue(id, out var def) ? def : null;

	public static ProcessDef GetProcess(string id) =>
		_processes.TryGetValue(id, out var def) ? def : null;

	public static List<IngredientDef> GetByCategory(string category)
	{
		var list = new List<IngredientDef>();
		foreach (var kv in _ingredients)
			if (kv.Value.Category == category)
				list.Add(kv.Value);
		return list;
	}

	public static List<ProcessDef> GetAllProcesses()
	{
		return new List<ProcessDef>(_processes.Values);
	}
}