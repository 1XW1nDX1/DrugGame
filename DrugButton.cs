using Godot;
using System;

public partial class DrugButton : Button
{
	[Export]public Node2D lines;
	public Drug drug;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (lines != null)
		{
			lines.Visible = false;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Choice()
	{
		if (lines != null)
		{
			lines.Visible = true;
		}
	}
}
