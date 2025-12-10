#This script is attached to a Button node.
extends Button
#Automatically call the function once when all the nodes and their children are loaded. 
func _ready():
	self.pressed.connect(_on_button_pressed)
# When the button is given a signal "pressed", call the function "_on_button_pressed".
# Define the function "_on_button_pressed", call the main tree, and execute the quit order. 
func _on_button_pressed():
	get_tree().quit()
