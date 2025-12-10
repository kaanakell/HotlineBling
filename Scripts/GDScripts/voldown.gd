extends Button

func _ready():
	self.pressed.connect(voldown)
	
func voldown():
	var master_bus = AudioServer.get_bus_index("Master")
	var current_vol = AudioServer.get_bus_volume_db(master_bus)

	var new_vol = max(current_vol - 3.0, -80.0)
	
	AudioServer.set_bus_volume_db(master_bus, new_vol)
	
	AudioServer.set_bus_mute(master_bus, new_vol <= -79.0)
