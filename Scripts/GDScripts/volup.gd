extends Button

func _ready():
	self.pressed.connect(volup)

func volup():
	var master_bus = AudioServer.get_bus_index("Master")
	var current_vol = AudioServer.get_bus_volume_db(master_bus)
	
	if AudioServer.is_bus_mute(master_bus):
		AudioServer.set_bus_mute(master_bus, false)

	var new_vol = min(current_vol + 3.0, 0.0)
	
	AudioServer.set_bus_volume_db(master_bus, new_vol)
