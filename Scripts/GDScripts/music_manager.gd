extends Node

# --- Load your Music Files ---
# Make sure you have these files in your Audio folder!
var menu_music = load("res://Audio/Music/Soundtrack_1.mp3") 
var game_music = load("res://Audio/Music/Soundtrack_2.mp3")

# New Tracks
var win_music = load("res://Audio/Music/Soundtrack_3.mp3")     # Change path if needed
var lose_music = load("res://Audio/Music/Soundtrack_3.mp3")   # Change path if needed
var pause_music = load("res://Audio/Music/Soundtrack_3.mp3") # Change path if needed

var music_player: AudioStreamPlayer

func _ready():
	music_player = AudioStreamPlayer.new()
	add_child(music_player)
	music_player.bus = "Master"
	
	# Process mode ALWAYS allows music to keep playing even when game is paused!
	self.process_mode = Node.PROCESS_MODE_ALWAYS

# --- Play Functions ---

func play_menu_music():
	_play_track(menu_music)

func play_game_music():
	_play_track(game_music)

func play_win_music():
	_play_track(win_music)

func play_lose_music():
	_play_track(lose_music)

func play_pause_music():
	_play_track(pause_music)

# --- Internal Logic ---
func _play_track(stream: AudioStream):
	if stream == null:
		return
		
	# If we are already playing this song, don't restart it
	if music_player.stream == stream and music_player.playing:
		return
	
	music_player.stream = stream
	music_player.play()
