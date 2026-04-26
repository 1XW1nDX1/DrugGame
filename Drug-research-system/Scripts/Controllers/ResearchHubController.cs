using Godot;

public partial class ResearchHubController : Control
{
	public ResearchTarget CurrentTarget { get; private set; }
	public DrugRecipe CurrentRecipe { get; private set; }
	public DrugPrototype CurrentPrototype { get; private set; }

	private readonly PrototypeGenerator _prototypeGenerator = new();

	public override void _Ready()
	{
		LoadTarget();
		CurrentRecipe = new DrugRecipe();
		RefreshView();
	}

	public void LoadTarget()
	{
		CurrentTarget = new ResearchTarget
		{
			TargetNeedType = "Bleeding",
			DeadlineRounds = 2,
			PreferredDrugFamily = "Hemostasis",
			Description = "Frontline bleeding cases are rising rapidly."
		};
	}

	public void UpdateRecipe(DrugRecipe recipe)
	{
		CurrentRecipe = recipe;
	}

	public void GeneratePrototype()
	{
		if (CurrentRecipe == null)
		{
			GD.Print("Recipe is missing.");
			return;
		}

		CurrentPrototype = _prototypeGenerator.BuildPrototype(CurrentRecipe);
		RefreshView();
	}

	public void RefreshView()
	{
		GD.Print("ResearchHub view refreshed.");
	}
}
