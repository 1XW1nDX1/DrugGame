using Godot;
using System;

public partial class ProductionLine : Node
{
	public string line_ID;
	public bool isUsed = false;
	public bool isSelected = false;
	[Export] public Drug currentdrug;
	private float product_speed = 1.0f;
	private float defect_rate = 0f;
	private float timer = 0f;

	public void Prepare(Drug drug)
	{
		currentdrug = drug;
		isUsed = drug != null;
		timer = 0f;
	}

	public void Release()
	{
		currentdrug = null;
		isUsed = false;
		timer = 0f;
	}

	public void Update(double delta)
	{
		if (!isUsed || currentdrug == null) return;

		timer += (float)delta;
		if (timer >= product_speed)
		{
			timer -= product_speed;
			Product(currentdrug);
		}
	}

	// Drug production
	void Product(Drug drug)
	{
		//Add functions
		drug.number++;
	}
}
