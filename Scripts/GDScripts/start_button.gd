extends Button

func _ready():
	self.pressed.connect(_on_button_pressed)

func _on_button_pressed():
	# Use call_deferred to wait for the current frame to finish
	# This helps avoid physics/resource conflicts during the switch
	call_deferred("change_scene")

func change_scene():
	var result = get_tree().change_scene_to_file("res://Scenes/game.tscn")
	
	if result != OK:
		printerr("Error loading game scene! Code: ", result)
