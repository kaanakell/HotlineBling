extends Area2D

#To use other nodes.
@export var EnemySystem: Node
@export var Layer2: Node
@export var Gate: Node
#When the game begins, this area is automatically unabled.
var entered = false

# When this Area2D enters the scene tree, connect the body_entered signal
# so that _on_body_entered() will run when a physics body enters this area.
func _ready():
	connect("body_entered", Callable(self, "_on_body_entered"))

func _on_body_entered(body):
	if entered:
		return
