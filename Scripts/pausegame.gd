extends Node

#================
#  Pause System
#================
@export var pause_menu: Control 

func _ready() -> void:
	if pause_menu:
		pause_menu.visible = false
		alive_enemies = 0
	#if an new enemy enters the scene, _on_child_added(), for transfering the number of the nodes into int for counting enemies.
	get_tree().connect("node_added", Callable(self, "_on_node_added"))
	self.process_mode = Node.PROCESS_MODE_ALWAYS

func _input(event: InputEvent) -> void:
#if Esc button is pressed, call the function toggle_pause.
	if event.is_action_pressed("ui_cancel"):
		toggle_pause()

func toggle_pause() -> void:
	var current_state = get_tree().paused
	
	get_tree().paused = not current_state
#To check the current state, if it's not pause, pause it, or vice versa.	
	if pause_menu:
		pause_menu.visible = not current_state
		

#================
#Enemy Counting System
#================
var alive_enemies: int = 0
	
func _on_node_added(node):
	#Mark all the enemies from all nodes.
	if node.is_in_group("enemies"):
		alive_enemies += 1
	# Listen for this enemy's death signal.
		node.connect("EnemyDied", Callable(self, "_on_enemy_died"))

func _on_enemy_died(enemy):
	alive_enemies -= 1
	
func all_cleared() -> bool:
	return alive_enemies <= 0 
