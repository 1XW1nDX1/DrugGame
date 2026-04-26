
using System.Collections.Generic;

public class DrugRecipe
{
	public string BaseIngredientId { get; set; } = string.Empty;

	public List<string> ActiveIngredientIds { get; set; } = new();

	public List<string> StabilizerIds { get; set; } = new();

	public List<string> ProcessIds { get; set; } = new();
}
