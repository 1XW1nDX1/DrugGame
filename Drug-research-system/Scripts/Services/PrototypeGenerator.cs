using System;

/// <summary>
/// 原型生成器 — 根据配方和成分表计算 DrugPrototype
/// </summary>
public class PrototypeGenerator
{
	public DrugPrototype BuildPrototype(DrugRecipe recipe)
	{
		var proto = new DrugPrototype
		{
			DrugId = "Prototype",
			VersionId = "Mk1",
			DrugFamily = recipe.BaseIngredientId
		};

		// 读取成分和工艺
		var active = IngredientDB.GetIngredient(
			recipe.ActiveIngredientIds.Count > 0 ? recipe.ActiveIngredientIds[0] : "");
		var stabilizer = recipe.StabilizerIds.Count > 0
			? IngredientDB.GetIngredient(recipe.StabilizerIds[0]) : null;
		var process = IngredientDB.GetProcess(
			recipe.ProcessIds.Count > 0 ? recipe.ProcessIds[0] : "");

		// 安全fallback
		float aEff = active?.EfficacyMod ?? 0;
		float aSpd = active?.SpeedMod ?? 0;
		float aSide = active?.SideEffectMod ?? 0;
		float sEff = stabilizer?.EfficacyMod ?? 0;
		float sSpd = stabilizer?.SpeedMod ?? 0;
		float sSta = stabilizer?.StabilityMod ?? 0;
		float sSide = stabilizer?.SideEffectMod ?? 0;
		float pSpd = process?.SpeedMod ?? 0;
		float pSta = process?.StabilityMod ?? 0;
		float pSide = process?.SideEffectMod ?? 0;
		float pCost = process?.CostMod ?? 0;

		// ── 核心计算 ──
		proto.Efficacy   = Clamp(40f + aEff + sEff);
		proto.OnsetSpeed = Clamp(40f + aSpd + pSpd + sSpd);
		proto.Duration   = 50f;
		proto.Stability  = Clamp(40f + sSta + pSta);
		proto.SideEffectIntensity = Clamp(20f + aSide + pSide + sSide);
		proto.MassProductionCost = Clamp(30f + pCost);
		proto.HiddenRisk = Clamp(
			proto.SideEffectIntensity * 0.4f + (100f - proto.Stability) * 0.3f);

		return proto;
	}

	private static float Clamp(float v) => Math.Clamp(v, 0f, 100f);
}