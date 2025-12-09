extends Area2D

@export var game_manager: Node 

var has_triggered: bool = false

func _ready() -> void:
	# Debug print to ensure script is running
	print("FloorExit: Ready and waiting for interaction...")
	body_entered.connect(_on_body_entered)

func _on_body_entered(body: Node2D) -> void:
	print("FloorExit: Something entered! Body name: ", body.name)
	
	if has_triggered:
		print("FloorExit: Already triggered, ignoring.")
		return
		
	# CHECK 1: Is this the Player?
	if not body.is_in_group("player"):
		print("FloorExit: Body is NOT in group 'Player'. Ignoring.")
		# Check what groups it IS in
		print("FloorExit: Body groups are: ", body.get_groups())
		return
		
	# CHECK 2: Is the Manager connected?
	if game_manager == null:
		printerr("FloorExit CRITICAL ERROR: 'Game Manager' is empty in the Inspector!")
		return

	# CHECK 3: Are enemies dead?
	if game_manager.are_all_enemies_dead():
		print("FloorExit: Conditions met! Transitioning...")
		has_triggered = true
		game_manager.call_deferred("go_to_floor_2")
	else:
		print("FloorExit: Door Locked. Enemies Remaining: ", game_manager.total_enemies)
