using Godot;
using System;

public partial class ProductionLine : Node
{
	public string line_ID;
	public bool isUsed = false;
	public bool isSelected = false;
	[Export] public Drug currentdrug;
	private float product_speed = 1.0f; //生产速度
	private float defect_rate = 0f;		//损坏率
	private float timer = 0f;

	//药品锁定+生产线初始化
	public void Prepare(Drug drug)
	{
		currentdrug = drug;
		isUsed = drug != null;
		timer = 0f;
	}

	public void SetCurrentDrug(Drug drug)
	{
		Prepare(drug);
	}

	//解除药品
	public void Release()
	{
		currentdrug = null;
		isUsed = false;
		timer = 0f;
	}

	//药品生产
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

	//药品生产方法实现
	private void Product(Drug drug)
	{
		//添加效果
		drug.number++;
	}
}
