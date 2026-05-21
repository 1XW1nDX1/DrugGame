using Godot;
using System;
using System.Collections.Generic;

public partial class LineManage : Node
{
	[Export] public Godot.Collections.Array<ProductionLine> ProductionLines;
	[Export] public Drug drugtoProduce;

	public int selectedLineIndex = -1;

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

	// Update the states of lines.
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

	//Choose the lines
	public bool SelectLine(int lineIndex)
	{
		if (ProductionLines == null || lineIndex < 0 || lineIndex >= ProductionLines.Count)
		{
			selectedLineIndex = -1;
			return false;
		}

		selectedLineIndex = lineIndex;
		return true;
	}

	public bool AssignSelectedLine()
	{
		if (selectedLineIndex < 0 || drugtoProduce == null) return false;
		return AssignLine(selectedLineIndex, drugtoProduce);
	}

	public bool AssignLine(int lineIndex, Drug drug)
	{
		if (ProductionLines == null || lineIndex < 0 || lineIndex >= ProductionLines.Count || drug == null)
		{
			return false;
		}

		var line = ProductionLines[lineIndex];
		if (line == null || (line.isUsed && line.currentdrug != null))
		{
			return false;
		}

		line.Prepare(drug);
		return true;
	}

	public void ReleaseLine(int lineIndex)
	{
		if (ProductionLines == null || lineIndex < 0 || lineIndex >= ProductionLines.Count) return;

		var line = ProductionLines[lineIndex];
		if (line != null)
		{
			line.Release();
		}
	}

	public string GetSelectedLineText()
	{
		if (ProductionLines == null || selectedLineIndex < 0 || selectedLineIndex >= ProductionLines.Count)
		{
			return "lines doesn't choosed";
		}

		return $": {ProductionLines[selectedLineIndex].line_ID}";
	}

	public void ProductManager()
	{
		ProductionLines = new Godot.Collections.Array<ProductionLine>();
	}

	public void InitialiseLine(int line_number)
	{
		ProductManager();
		for (int i = 0; i < line_number; i++)
		{
			ProductionLines.Add(new ProductionLine {line_ID = $"Line{i+1}", isUsed = false });
		}
	}
}