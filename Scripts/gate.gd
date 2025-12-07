extends StaticBody2D

func open_gate():
	collision_layer = 0
	collision_mask = 0
	queue_free()
	
