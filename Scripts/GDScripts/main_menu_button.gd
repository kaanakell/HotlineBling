extends Button

func _ready():
	self.pressed.connect(_on_pressed)

func _on_pressed():
	# 1. Unpause the game (Critical!)
	get_tree().paused = false
	
	# 2. Load the Main Menu scene
	# Make sure this path matches your Main Menu file exactly!
	get_tree().change_scene_to_file("res://Scenes/MainMenu.tscn")
	
	# 3. Find and destroy the Win/Lose Screen overlay
	# We climb up the tree to find the root CanvasLayer (WinUI or LoseUI)
	var root_node = self
	while root_node.get_parent() != null and not (root_node is CanvasLayer):
		root_node = root_node.get_parent()
		
	if root_node:
		root_node.queue_free()
