extends Button

func _ready():
	self.process_mode = Node.PROCESS_MODE_WHEN_PAUSED
	self.pressed.connect(_on_resume_clicked)

func _on_resume_clicked():
	get_tree().paused = false

	var menu_panel = get_parent()
	
	while menu_panel and not menu_panel.name == "Settings":
		menu_panel = menu_panel.get_parent()
		
	if menu_panel:
		menu_panel.visible = false
