using Godot;
using System;

namespace Game; // class_name

public partial class Main : Node2D
{

	private Sprite2D cursor;
	private PackedScene buildScene;
	private Button placeBuildingButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		buildScene = GD.Load<PackedScene>("res://scenes/building/building.tscn");
		cursor = GetNode<Sprite2D>("Cursor"); // Só o sprite, não o cursor em si
		placeBuildingButton = GetNode<Button>("PlaceBuildButton");
		cursor.Visible = false;

		// Já estou pegando o sinal, e quando ele apitar chamarei a função OnButtonPressed.
		//placeBuildingButton.Connect(Button.SignalName.Pressed, Callable.From(OnButtonPressed));
		placeBuildingButton.Pressed += OnButtonPressed; //Assign this method as a listener of this event (button_pressed)
		
	}

	 public override void _UnhandledInput(InputEvent evt)
    {
       if(cursor.Visible && evt.IsActionPressed("left_click"))
	   {
			PlaceBuildingAtMousePosition();
			cursor.Visible = false;
	   }
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var gridPosition = GetMouseGrideCellPosition();

		// Multiplicamos o gridPosition por 64 para o sprite andar 64pixels por cada 1 pixel do mouse
		cursor.GlobalPosition = gridPosition * 64;
	}

	private Vector2 GetMouseGrideCellPosition()
	{
		var mousePosition = GetGlobalMousePosition();
		var gridPosition = mousePosition / 64; // x/64 e y/64
		// Isso retornara os dois vetores da posição do mouse arredondado pra baixo, assim o canto superior esquerdo da tela é 0,0
		gridPosition = gridPosition.Floor(); 
		return gridPosition;
	}
   
   	private void PlaceBuildingAtMousePosition()
	{
		//Ao inves de usar var posso passar o tipo da variável
		Node2D building = buildScene.Instantiate<Node2D>();
		AddChild(building);

		var gridPosition = GetMouseGrideCellPosition();
		building.GlobalPosition = gridPosition * 64;
	}

	private void OnButtonPressed()
	{
		cursor.Visible = true;
	}
}
