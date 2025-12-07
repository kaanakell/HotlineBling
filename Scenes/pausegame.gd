extends Node

#pause_menu
@onready var pause_menu: Control = get_node("UI/Settings")
#@onready var music: AudioStreamPlayer = get_node("music")

var is_paused:bool = false

func _ready() -> void:
	pause_menu.visible = false
	pause_menu.process_mode= Node.PROCESS_MODE_WHEN_PAUSED
	#music.process_mode = Node.PROCESS_MODE_WHEN_ALWAYS
	
func _input(event: InputEvent) -> void:
	if event.is_action_pressed("ui_cancel"):
		toggle_pause()

func toggle_pause()-> void:
	is_paused = not is_paused
	get_tree().paused = is_paused
	pause_menu.visible = is_paused



		
	
	
	
