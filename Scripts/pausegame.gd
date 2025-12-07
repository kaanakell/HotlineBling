extends Node
@export var pause_menu: Control 

func _ready() -> void:
	if pause_menu:
		pause_menu.visible = false
	
	self.process_mode = Node.PROCESS_MODE_ALWAYS

func _input(event: InputEvent) -> void:
	if event.is_action_pressed("ui_cancel"):
		toggle_pause()

func toggle_pause() -> void:
	var current_state = get_tree().paused
	
	get_tree().paused = not current_state
	
	if pause_menu:
		pause_menu.visible = not current_state
