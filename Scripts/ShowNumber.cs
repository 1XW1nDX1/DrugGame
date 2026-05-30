using Godot;
using System;

public partial class ShowNumber : Label
{
	[Export] public Drug drug;
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Text = "药物数量" + drug.number;
	}
}
