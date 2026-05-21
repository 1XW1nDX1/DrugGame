using Godot;
using System;

public partial class DrugUI : Node
{
	[Export]public Drug drug;
	[Export]public Label number_label;
	public override void _Ready()
	{
		ShowUI();
	}

	public override void _Process(double delta)
	{
		ShowUI();
	}

	private void ShowUI()
	{
		if (number_label != null && drug != null)
		{
			number_label.Text = $"drugnumber = {drug.number}";
		}
		else
		{
			GD.PrintErr("wanglawangle");
		}
	}
}
