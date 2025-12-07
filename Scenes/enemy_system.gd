extends Node

var spawn_finished: bool = false

var alive_enemies: int = 0

@onready var gate := get_node("../../GameWorld/Layer1/Gate")

func _ready():
	spawn_finished = false
	alive_enemies = 0
	#if an new enemy enters the scene, _on_child_added(), for transfering the number of the nodes into int for counting enemies.
	get_tree().connect("node_added", Callable(self, "_on_node_added"))

func _on_node_added(node):
	#Mark all the enemies from all nodes.
	if node.is_in_group("enemies"):
		alive_enemies += 1
	# When an enemy dies, add on a node to transfer the numver of the enemy node into int to count dead enemies.
		node.connect("EnemyDied", Callable(self, "EnemyDied"))

func _on_enemy_died(enemy):
	alive_enemies -= 1
	check_clear_condition()

func finishing_spawning():
	spawn_finished = true
	check_clear_condition()
	
func check_clear_condition():
	if alive_enemies == 0 and spawn_finished:
		gate.call("open_gate")
		
		
