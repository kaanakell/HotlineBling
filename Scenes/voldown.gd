extends Button

func _ready():
	self.pressed.connect(voldown)
	
func voldown():
	var master_bus = AudioServer.get_bus_index("Master")
	var volume = AudioServer.get_bus_volume_db(master_bus)
	AudioServer.set_bus_volume_db(master_bus, min(volume - 3.0, -80.0))
	
