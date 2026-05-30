using Godot;
using System;
using System.Collections.Generic;

public partial class GameManager : Node
{
	[Export] public Godot.Collections.Array<LineUI> Lines;
	[Export] public DrugUI drug_UI;
	public Drug drugtoproduce;
	private LineUI selectedLine;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		drugtoproduce = null;
		BuildLineLinkedList();
		SetLinePanelVisible(false);
	}

	private void BuildLineLinkedList()
	{
		if (Lines == null || Lines.Count == 0)
		{
			return;
		}

		for (int i = 0; i < Lines.Count; i++)
		{
			var current = Lines[i];
			if (current == null)
			{
				continue;
			}

			current.gameManager = this;
			current.Next = i + 1 < Lines.Count ? Lines[i + 1] : null;
			current.Previous = i - 1 >= 0 ? Lines[i - 1] : null;
		}
	}

	private void SetLinePanelVisible(bool visible)
	{
		if (Lines == null) return;

		foreach (var line in Lines)
		{
			if (line != null)
			{
				line.Visible = visible;
			}
		}
	}

	//药物选择实现
	public void Drug_Choosed()
	{
		drugtoproduce = drug_UI?.drug;
		if (drugtoproduce != null)
		{
			SetLinePanelVisible(true);
		}
	}

	//生产线选择
	public void Line_Choosed(LineUI chosenLine)
	{
		if (chosenLine == null || drugtoproduce == null)
		{
			return;
		}

		selectedLine?.SetSelected(false);
		selectedLine = chosenLine;
		selectedLine.SetSelected(true);
		selectedLine.productionLine.Prepare(drugtoproduce);
		drugtoproduce = null;
		SetLinePanelVisible(false);
	}

	public void Line_Choosed()
	{
		if (Lines != null && Lines.Count > 0)
		{
			Line_Choosed(Lines[0]);
		}
	}
}
