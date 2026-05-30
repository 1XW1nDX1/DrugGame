using Godot;
using System;
using System.Collections.Generic;

public partial class LineManage : Node
{
	[Export] public Godot.Collections.Array<ProductionLine> ProductionLines;

	public override void _Ready()
	{
		if (ProductionLines == null)
		{
			ProductionLines = new Godot.Collections.Array<ProductionLine>();
		}

		for (int i = 0; i < ProductionLines.Count; i++)
		{
			var line = ProductionLines[i];
			if (line != null)
			{
				line.isUsed = line.currentdrug != null;
			}
		}
	}

	//更新生产线状态
	public override void _Process(double delta)
	{
		if (ProductionLines == null) return;

		foreach (var line in ProductionLines)
		{
			if (line != null)
			{
				line.Update(delta);
			}
		}
	}

	public void InitialiseLine(int line_number)
	{
		ProductionLines = new Godot.Collections.Array<ProductionLine>();
		for (int i = 0; i < line_number; i++)
		{
			ProductionLines.Add(new ProductionLine { line_ID = $"Line{i+1}", isUsed = false });
		}
	}
}