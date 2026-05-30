using Godot;
using System;

public partial class ResearchHubController : Control
{
	// 当前数据
	public ResearchTarget CurrentTarget { get; private set; }
	public DrugRecipe CurrentRecipe { get; private set; }
	public DrugPrototype CurrentPrototype { get; private set; }
	public ApprovalResult CurrentApproval { get; private set; }
	public FinishedDrug CurrentFinishedDrug { get; private set; }

	private readonly PrototypeGenerator _prototypeGenerator = new();
	private static int _drugSeq = 0;

	// ── UI 节点 ──
	private OptionButton _stabilizerOption;
	private OptionButton _processOption;
	private Button _generateBtn;
	private Label _resultLabel;
	private Button _fullTestBtn;
	private Button _simplifiedTestBtn;
	private Button _emergencyBtn;
	private Label _approvalLabel;
	private Label _finishedLabel;
	private Button _confirmBtn;

	public override void _Ready()
	{
		LoadTarget();
		CurrentRecipe = new DrugRecipe
		{
			BaseIngredientId = "HemostasisBase",
			ActiveIngredientIds = new() { "RapidClot" }
		};
		SetupUI();
		RefreshView();
	}

	private void LoadTarget()
	{
		CurrentTarget = new ResearchTarget
		{
			TargetNeedType = "Bleeding",
			DeadlineRounds = 2,
			PreferredDrugFamily = "Hemostasis",
			Description = "前线失血伤亡激增，需尽快提供止血药剂"
		};
	}

	// ── 搭建 UI（代码生成，不依赖 .tscn 手动拖节点）──
	private void SetupUI()
	{
		var container = new VBoxContainer();
		container.Position = new Vector2(20, 20);
		AddChild(container);

		// 标题
		container.AddChild(MakeLabel("药剂研发台 — 止血系列", 24));

		// 需求信息
		container.AddChild(MakeLabel("当前需求：前线失血伤亡激增（剩余 2 回合）", 14));

		// 固定成分显示
		container.AddChild(MakeLabel("基础成分：止血基底（固定）", 14));
		container.AddChild(MakeLabel("活性成分：速凝提取物（固定）", 14));

		// 稳定成分选择
		container.AddChild(MakeLabel("稳定成分：", 14));
		_stabilizerOption = new OptionButton();
		_stabilizerOption.AddItem("无", 0);
		foreach (var def in IngredientDB.GetByCategory("Stabilizer"))
			_stabilizerOption.AddItem(def.DisplayName);
		container.AddChild(_stabilizerOption);

		// 工艺选择
		container.AddChild(MakeLabel("工艺选择：", 14));
		_processOption = new OptionButton();
		foreach (var def in IngredientDB.GetAllProcesses())
			_processOption.AddItem(def.DisplayName);
		container.AddChild(_processOption);

		// 生成按钮
		_generateBtn = new Button();
		_generateBtn.Text = "生成原型";
		_generateBtn.Pressed += OnGenerate;
		container.AddChild(_generateBtn);

		// 原型结果
		_resultLabel = MakeLabel("", 14);
		container.AddChild(_resultLabel);

		// 审批按钮
		var approvalRow = new HBoxContainer();
		_fullTestBtn = MakeButton("完整测试(3回合)");
		_simplifiedTestBtn = MakeButton("简化测试(1回合)");
		_emergencyBtn = MakeButton("紧急授权(0回合)");
		_fullTestBtn.Pressed += () => OnApprove(ApprovalService.Mode.FullTest);
		_simplifiedTestBtn.Pressed += () => OnApprove(ApprovalService.Mode.SimplifiedTest);
		_emergencyBtn.Pressed += () => OnApprove(ApprovalService.Mode.Emergency);
		approvalRow.AddChild(_fullTestBtn);
		approvalRow.AddChild(_simplifiedTestBtn);
		approvalRow.AddChild(_emergencyBtn);
		container.AddChild(approvalRow);

		// 审批结果
		_approvalLabel = MakeLabel("", 14);
		container.AddChild(_approvalLabel);

		// 成品信息
		_finishedLabel = MakeLabel("", 14);
		container.AddChild(_finishedLabel);

		// 确认生产按钮
		_confirmBtn = new Button();
		_confirmBtn.Text = "确认生产（输出 FinishedDrug）";
		_confirmBtn.Pressed += OnConfirm;
		container.AddChild(_confirmBtn);
	}

	// ── 逻辑 ──
	private void OnGenerate()
	{
		// 读取 UI 选择
		CurrentRecipe.StabilizerIds.Clear();
		CurrentRecipe.ProcessIds.Clear();

		// 稳定成分
		int stabIdx = _stabilizerOption.Selected;
		if (stabIdx > 0) // 0 = 无
		{
			var stabilizers = IngredientDB.GetByCategory("Stabilizer");
			if (stabIdx - 1 < stabilizers.Count)
				CurrentRecipe.StabilizerIds.Add(stabilizers[stabIdx - 1].Id);
		}

		// 工艺
		int procIdx = _processOption.Selected;
		var processes = IngredientDB.GetAllProcesses();
		if (procIdx >= 0 && procIdx < processes.Count)
			CurrentRecipe.ProcessIds.Add(processes[procIdx].Id);

		// 生成原型
		CurrentPrototype = _prototypeGenerator.BuildPrototype(CurrentRecipe);
		CurrentApproval = null;
		CurrentFinishedDrug = null;

		RefreshView();
	}

	private void OnApprove(ApprovalService.Mode mode)
	{
		if (CurrentPrototype == null) return;

		CurrentApproval = ApprovalService.Approve(CurrentPrototype, mode);
		CurrentFinishedDrug = BuildFinishedDrug(CurrentPrototype, CurrentApproval, mode);

		RefreshView();
	}

	private void OnConfirm()
	{
		if (CurrentFinishedDrug == null)
		{
			GD.Print("请先生成原型并选择审批方式");
			return;
		}

		GD.Print($"=== 药品成品确认 ===");
		GD.Print($"DrugId: {CurrentFinishedDrug.DrugId}");
		GD.Print($"DrugName: {CurrentFinishedDrug.DrugName}");
		GD.Print($"DrugFamily: {CurrentFinishedDrug.DrugFamily}");
		GD.Print($"ApprovalMode: {CurrentFinishedDrug.ApprovalMode}");
		GD.Print($"TestCompletion: {CurrentFinishedDrug.TestCompletion}");
		GD.Print($"RiskGrade: {CurrentFinishedDrug.RiskGrade}");
		GD.Print($"Efficacy: {CurrentFinishedDrug.Efficacy}");
		GD.Print($"OnsetSpeed: {CurrentFinishedDrug.OnsetSpeed}");
		GD.Print($"Duration: {CurrentFinishedDrug.Duration}");
		GD.Print($"Stability: {CurrentFinishedDrug.Stability}");
		GD.Print($"SideEffect: {CurrentFinishedDrug.SideEffect}");
		GD.Print($"BaseCost: {CurrentFinishedDrug.BaseCost}");
		GD.Print($"RequiredMaterials: [{string.Join(", ", CurrentFinishedDrug.RequiredMaterials)}]");
		GD.Print($"HiddenRisk: {CurrentFinishedDrug.HiddenRisk}");
		GD.Print($"====================");
	}

	private FinishedDrug BuildFinishedDrug(DrugPrototype proto, ApprovalResult approval, ApprovalService.Mode mode)
	{
		_drugSeq++;
		string modeStr = mode switch
		{
			ApprovalService.Mode.FullTest => "FullTest",
			ApprovalService.Mode.SimplifiedTest => "SimplifiedTest",
			_ => "Emergency"
		};

		// 生成名称
		string stabilizerName = CurrentRecipe.StabilizerIds.Count > 0
			? IngredientDB.GetIngredient(CurrentRecipe.StabilizerIds[0])?.DisplayName ?? ""
			: "";
		string processName = CurrentRecipe.ProcessIds.Count > 0
			? IngredientDB.GetProcess(CurrentRecipe.ProcessIds[0])?.DisplayName ?? ""
			: "";
		string name = "止血剂";
		if (!string.IsNullOrEmpty(stabilizerName) && stabilizerName != "无")
			name = stabilizerName.Replace("温和", "").Replace("剂", "") + name;
		if (!string.IsNullOrEmpty(processName))
			name = processName + name;

		return new FinishedDrug
		{
			DrugId = $"Drug_{_drugSeq:D3}",
			DrugName = name,
			DrugFamily = proto.DrugFamily,
			ApprovalMode = modeStr,
			TestCompletion = approval.TestCompletion,
			RiskGrade = approval.RiskGrade,
			Efficacy = proto.Efficacy,
			OnsetSpeed = proto.OnsetSpeed,
			Duration = proto.Duration,
			Stability = proto.Stability,
			SideEffect = proto.SideEffectIntensity,
			BaseCost = proto.MassProductionCost,
			RequiredMaterials = new[] { "Herb_Common", "Solvent_Basic" },
			HiddenRisk = approval.FinalHiddenRisk
		};
	}

	private void RefreshView()
	{
		if (CurrentPrototype != null)
		{
			_resultLabel.Text =
				$"── 原型参数 ──\n" +
				$"疗效: {CurrentPrototype.Efficacy:F0}  " +
				$"速度: {CurrentPrototype.OnsetSpeed:F0}  " +
				$"持续: {CurrentPrototype.Duration:F0}\n" +
				$"稳定: {CurrentPrototype.Stability:F0}  " +
				$"副作用: {CurrentPrototype.SideEffectIntensity:F0}  " +
				$"成本: {CurrentPrototype.MassProductionCost:F0}\n" +
				$"隐藏风险: {CurrentPrototype.HiddenRisk:F0}";
		}
		else
		{
			_resultLabel.Text = "尚未生成原型";
		}

		if (CurrentApproval != null)
		{
			_approvalLabel.Text =
				$"── 审批结果 ──\n" +
				$"测试完成度: {CurrentApproval.TestCompletion:F0}%  " +
				$"风险等级: {CurrentApproval.RiskGrade}\n" +
				$"最终隐藏风险: {CurrentApproval.FinalHiddenRisk:F0}\n" +
				$"{CurrentApproval.ApprovalNote}";
		}
		else
		{
			_approvalLabel.Text = "尚未审批";
		}

		if (CurrentFinishedDrug != null)
		{
			_finishedLabel.Text =
				$"── 成品信息 ──\n" +
				$"ID: {CurrentFinishedDrug.DrugId}  " +
				$"药名: {CurrentFinishedDrug.DrugName}\n" +
				$"原料: [{string.Join(", ", CurrentFinishedDrug.RequiredMaterials)}]  " +
				$"单剂成本: {CurrentFinishedDrug.BaseCost:F0}\n" +
				$"点击「确认生产」输出到控制台";
		}
		else
		{
			_finishedLabel.Text = "";
		}
	}

	// ── UI 工具方法 ──
	private static Label MakeLabel(string text, int fontSize)
	{
		var label = new Label();
		label.Text = text;
		label.AddThemeFontSizeOverride("font_size", fontSize);
		return label;
	}

	private static Button MakeButton(string text)
	{
		var btn = new Button();
		btn.Text = text;
		return btn;
	}
}