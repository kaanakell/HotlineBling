extends Button

func _ready():
	self.pressed.connect(restart)

func restart():
	get_tree().paused = true
	get_tree().call_deferred("reload_current_scene")
	#R to restart
	
