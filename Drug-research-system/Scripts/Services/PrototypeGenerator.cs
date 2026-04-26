public class PrototypeGenerator
{
	public DrugPrototype BuildPrototype(DrugRecipe recipe)
	{
		return new DrugPrototype
		{
			DrugId = "Prototype",
			VersionId = "Mk1",
			DrugFamily = recipe.BaseIngredientId,
			Efficacy = 50.0f,
			OnsetSpeed = recipe.ActiveIngredientIds.Count > 0 ? 60.0f : 40.0f,
			Duration = 45.0f,
			Stability = recipe.StabilizerIds.Count > 0 ? 55.0f : 35.0f,
			SideEffectIntensity = recipe.ActiveIngredientIds.Count > 1 ? 45.0f : 25.0f
		};
	}
}
