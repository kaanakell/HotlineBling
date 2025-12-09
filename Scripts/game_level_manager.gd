extends Node

# --- Configuration ---
@export var pause_menu: Control

@export var floor_1_root: Node2D
@export var floor_2_root: Node2D
@export var floor_2_spawn: Marker2D
@export var player: CharacterBody2D

# --- State ---
var total_enemies: int = 0

func _ready() -> void:
	# 1. Setup Pause Menu
	if pause_menu:
		pause_menu.visible = false
	self.process_mode = Node.PROCESS_MODE_ALWAYS # Manager must run while paused!

	# 2. Setup Floors (Start on Floor 1)
	change_floor_state(floor_1_root, true)
	change_floor_state(floor_2_root, false)

	# 3. Count Enemies cleanly
	_count_initial_enemies()

func _input(event: InputEvent) -> void:
	if event.is_action_pressed("ui_cancel"): # Escape key
		toggle_pause()

# --- Pause System ---
func toggle_pause() -> void:
	var is_paused = get_tree().paused
	get_tree().paused = not is_paused
	
	if pause_menu:
		pause_menu.visible = not is_paused

# --- Floor Management (The Fix!) ---
# This function fixes the bug where hidden enemies kept attacking.
func change_floor_state(floor_node: Node2D, is_active: bool) -> void:
	if floor_node == null:
		return
		
	# Visibility: Hides the sprites
	floor_node.visible = is_active
	
	# Process Mode: Stops code, physics, and collisions!
	if is_active:
		floor_node.process_mode = Node.PROCESS_MODE_INHERIT
	else:
		floor_node.process_mode = Node.PROCESS_MODE_DISABLED

func go_to_floor_2() -> void:
	print("Changing to Floor 2...")
	change_floor_state(floor_1_root, false) # Disable Floor 1
	change_floor_state(floor_2_root, true)  # Enable Floor 2
	
	if player and floor_2_spawn:
		player.global_position = floor_2_spawn.global_position

# --- Enemy System ---
func _count_initial_enemies() -> void:
	# Instead of "node_added", we grab everyone already in the group
	var enemies = get_tree().get_nodes_in_group("enemies")
	total_enemies = enemies.size()
	
	print("Level Started. Enemies to kill: ", total_enemies)
	
	# Connect to their death signals
	for enemy in enemies:
		if not enemy.is_connected("tree_exited", _on_enemy_killed):
			enemy.tree_exited.connect(_on_enemy_killed)

# We use 'tree_exited' because it works even if you verify queue_free()
func _on_enemy_killed() -> void:
	total_enemies -= 1
	print("Enemy Down! Remaining: ", total_enemies)
	
	if total_enemies <= 0:
		print("FLOOR CLEARED! Door unlocked.")

func are_all_enemies_dead() -> bool:
	return total_enemies <= 0
