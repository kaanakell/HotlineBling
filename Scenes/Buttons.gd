extends Control

#Get all the buttons ready when game starts.
func _ready():
	print("Ready called")
	$StartButton.pressed.connect(_on_StartButton_pressed)

func _on_StartButton_pressed():
	print("game starts")	
	
