extends Node

# --- Configuration ---
@export var pause_menu: CanvasLayer

@export var floor_1_root: Node2D
@export var floor_2_root: Node2D
@export var floor_2_spawn: Marker2D
@export var player: CharacterBody2D

# --- State ---
var enemies_remaining_on_current_floor: int = 0
var current_active_floor: Node2D

func _ready() -> void:
	Input.set_mouse_mode(Input.MOUSE_MODE_HIDDEN)
	if pause_menu:
		pause_menu.visible = false
	self.process_mode = Node.PROCESS_MODE_ALWAYS 
	MusicManager.play_game_music()

	# Start on Floor 1
	start_floor(floor_1_root)
	change_floor_state(floor_2_root, false)

func _input(event: InputEvent) -> void:
	if event.is_action_pressed("ui_cancel"):
		toggle_pause()

# --- Floor Management ---
func start_floor(floor_node: Node2D) -> void:
	print("Starting Floor: ", floor_node.name)
	current_active_floor = floor_node
	
	# 1. Activate the floor visually/physically
	change_floor_state(floor_node, true)
	
	# 2. Count ONLY the enemies inside this floor node
	count_enemies_in_floor(floor_node)

func change_floor_state(floor_node: Node2D, is_active: bool) -> void:
	if floor_node == null: return
	floor_node.visible = is_active
	floor_node.process_mode = Node.PROCESS_MODE_INHERIT if is_active else Node.PROCESS_MODE_DISABLED

func go_to_floor_2() -> void:
	print("Transitioning to Floor 2...")
	
	# Disable Floor 1
	change_floor_state(floor_1_root, false)
	
	# Move Player
	if player and floor_2_spawn:
		player.global_position = floor_2_spawn.global_position
		
	# Start Floor 2 (This will reset the count for the new 15 enemies)
	start_floor(floor_2_root)

# --- Enemy Counting System ---
func count_enemies_in_floor(floor_node: Node2D) -> void:
	enemies_remaining_on_current_floor = 0
	
	# Get EVERY enemy in the game
	var all_enemies = get_tree().get_nodes_in_group("enemies")
	
	# Filter: Only keep the ones that are children/descendants of the current floor
	for enemy in all_enemies:
		if floor_node.is_ancestor_of(enemy):
			enemies_remaining_on_current_floor += 1
			
			# Connect signal if not already connected
			if not enemy.is_connected("tree_exited", _on_enemy_killed):
				enemy.tree_exited.connect(_on_enemy_killed)
	
	print("Floor Started. Enemies to kill: ", enemies_remaining_on_current_floor)

func _on_enemy_killed() -> void:
	enemies_remaining_on_current_floor -= 1
	print("Enemy Down! Remaining on this floor: ", enemies_remaining_on_current_floor)
	
	if enemies_remaining_on_current_floor <= 0:
		print("FLOOR CLEARED!")
		# You can play a sound effect here "Door Unlocked"

# Used by the Door Trigger to check if we can pass
func are_all_enemies_dead() -> bool:
	return enemies_remaining_on_current_floor <= 0

# --- Pause System ---
func toggle_pause() -> void:
	var is_paused = get_tree().paused
	get_tree().paused = not is_paused
	
	if pause_menu:
		pause_menu.visible = not is_paused
	
	if not is_paused:
		Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
		MusicManager.play_pause_music()
	else:
		Input.set_mouse_mode(Input.MOUSE_MODE_HIDDEN)
		MusicManager.play_game_music()
