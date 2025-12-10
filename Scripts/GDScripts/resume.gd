extends Button

func _ready():
	self.pressed.connect(_on_resume_pressed)

func _on_resume_pressed():
	get_tree().paused = false
	Input.set_mouse_mode(Input.MOUSE_MODE_HIDDEN)
	
	var menu_root = owner
	if menu_root:
		menu_root.queue_free()
	else:
		get_parent().get_parent().get_parent().queue_free()
