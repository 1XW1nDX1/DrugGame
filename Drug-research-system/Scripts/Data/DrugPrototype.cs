public class DrugPrototype
{
	public string DrugId { get; set; } = string.Empty;

	public string VersionId { get; set; } = string.Empty;

	public string DrugFamily { get; set; } = string.Empty;

	public float Efficacy { get; set; }

	public float OnsetSpeed { get; set; }

	public float Duration { get; set; }

	public float Stability { get; set; }

	public float SideEffectIntensity { get; set; }
}
