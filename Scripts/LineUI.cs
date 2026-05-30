using Godot;
using System;

public partial class LineUI : Button
{
	[Export] public ProductionLine productionLine;
	[Export] public GameManager gameManager;

	public LineUI Next { get; set; }
	public LineUI Previous { get; set; }

	private readonly Color normalColor = Colors.White;
	private readonly Color selectedColor = new Color(0.7f, 0.9f, 1.0f, 1.0f);

	public override void _Ready()
	{
		UpdateVisualState();
	}

	public void SetSelected(bool selected)
	{
		if (productionLine != null)
		{
			productionLine.isSelected = selected;
		}
		UpdateVisualState();
	}

	public void OnPressed()
	{
		gameManager?.Line_Choosed(this);
	}

	private void UpdateVisualState()
	{
		SelfModulate = productionLine != null && productionLine.isSelected ? selectedColor : normalColor;
	}
}
