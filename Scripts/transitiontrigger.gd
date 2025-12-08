#================
#Map Transition System
#================
extends Area2D

#To use other nodes.
@export var gamemanager: Node2D
@export var entry: Marker2D
@export var player :CharacterBody2D

#When the game begins, this area is automatically disabled.
var entered = false
# When this Area2D enters the scene tree, connect the body_entered signal
# so that _on_body_entered() will run when a physics body enters this area.
func _ready():
	connect("body_entered", Callable(self, "_on_body_entered"))

func _on_body_entered(body):
	if entered:
		return
	if not body.is_in_group("player"):
		return
	if not gamemanager.all_cleared():
		return
	entered = true
	_teleport_player()
	
func _teleport_player():
	if entry and player:
		player.global_position = entry.global_position

		
