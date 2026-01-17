using Godot;
using System.Collections.Generic;
using GDPlaylistArray = Godot.Collections.Array<SteppeAttorney.Audio.Playlist>;
using GDPlayerDict = Godot.Collections.Dictionary<SteppeAttorney.Audio.PlaylistType, Godot.AudioStreamPlayer>;
using GDictStringToTrack = Godot.Collections.Dictionary<string, SteppeAttorney.Audio.Track>;
using GDBusDict = Godot.Collections.Dictionary<SteppeAttorney.Audio.PlaylistType, string>;

namespace SteppeAttorney.Audio;

public struct PlaylistTypeData(Playlist playlist, AudioStreamPlayer player, string busName)
{
    public Playlist Playlist = playlist;
    public AudioStreamPlayer Player = player;
    public string BusName = busName;
}

public partial class AudioPlayer : Node
{
    [Signal] public delegate void ChangeCurrentMusicEventHandler(string newName);

    [Export] private GDPlaylistArray Playlists { get; set; } = [];
    [Export] private GDPlayerDict Players { get; set; } = [];
    [Export] private GDBusDict BusNames { get; set; } = [];

    private readonly Dictionary<PlaylistType, PlaylistTypeData> _finder = [];

    public static AudioPlayer? Instance { get; private set; }

    public override void _Ready()
    {
        if (Instance != null)
        {
            QueueFree(); 
            return;
        }

        Instance = this;

        foreach (Playlist playlist in Playlists)
        {
            if (!Players.TryGetValue(playlist.Type, out var player)) 
            {
                GD.PrintErr($"Couldn't get Player with Type {playlist.Type}!");
                continue; 
            }
            if (!BusNames.TryGetValue(playlist.Type, out var busName)) GD.PrintErr($"Couldn't find bus name for type {playlist.Type}.");
            busName ??= "Master";
            player.Name = playlist.Type.ToString();
            IsBusName(busName, out var bus);
            player.Bus = bus;
            _finder.Add(playlist.Type, new PlaylistTypeData(playlist, player, busName));
        }
    }

    private static bool IsBusName(string busName, out string name)
    {
        if (AudioServer.GetBusIndex(busName) == -1)
        {
            name = "Master";
            GD.PrintErr($"Couldn't find bus: {busName}");
            return false;
        }
        name = busName;
        return true;
    }

    public bool? IsPlaying(PlaylistType type) => Find(type)?.Player.Playing;
    public Track GetCurrentTrack(PlaylistType type) 
    {
        var playlistData = Find(type);
        var tracks = playlistData?.Playlist.Tracks;
        var stream = playlistData?.Player.Stream;

        return tracks.GetTrackByStream(stream);
    }
    public Track GetTrackFromTechName(string name, PlaylistType type) => GetTrackDict(type).GetTrackByKey(name);
    public string GetTechNameFromStream(AudioStream stream, PlaylistType type) => GetTrackDict(type).GetKeyByStream(stream);
    public string GetTechNameFromTrack(Track track, PlaylistType type) => GetTrackDict(type).GetKeyByTrack(track);

    private GDictStringToTrack GetTrackDict(PlaylistType type) => Find(type)?.Playlist.Tracks;
    private PlaylistTypeData? Find(PlaylistType type)
    {
        if (_finder.TryGetValue(type, out var data))
            return data;

        GD.PrintErr($"Playlist type {type} not found");
        return null;
    }

    public void Play(Track track, PlaylistType type, float fromPosition = 0)
    {
        var player = Find(type)?.Player;
        Play(track.Audio, player, fromPosition);
        if (type == PlaylistType.Music) PronounceNewName(track.Name);
    }

    private static void Play(AudioStream stream, AudioStreamPlayer player, float fromPosition = 0)
    {
        if (player.Stream == stream) return;
        player.Stop();
        player.Stream = stream;
        player.Play(fromPosition);
    }

    public void FadeIn(Track track, PlaylistType type, float fadein, float fromPosition = 0)
    {
        var player = Find(type)?.Player;
        if (player == null) return;
        player.VolumeDb = -80.0f;
        Play(track.Audio, player, fromPosition);
        if (type == PlaylistType.Music) PronounceNewName(track.Name);
        Tween twn = CreateTween();
        twn.TweenProperty(player, "volume_db", 0.0f, fadein);
    }

    private void PronounceNewName(string name) => EmitSignal(SignalName.ChangeCurrentMusic, name);
   
    public void Stop(PlaylistType type)
    {
        Find(type)?.Player.Stop();
        if (type == PlaylistType.Music) PronounceNewName("");
    }

    public void FadeOut(float fadeout, PlaylistType type)
    {
        var player = Find(type)?.Player;
        Tween twn = CreateTween();
        twn.TweenProperty(player, "volume_db", -80.0, fadeout);
        twn.TweenCallback(Callable.From(player.Stop));
        twn.TweenCallback(Callable.From(() => player.VolumeDb = 0.0f));
    }

    public void ChangeParameter(PlaylistType type, NodePath property, Variant variant, double duration = 0)
    {
        Tween tween = CreateTween();
        tween.TweenProperty(Find(type)?.Player, property, variant, duration);
    }

    public void SetVolume(float volume, PlaylistType type, double duration = 0)
    {
        Tween tween = CreateTween();
        tween.TweenProperty(Find(type)?.Player, "volume_db", volume, duration);
    }

    public float? GetVolume(PlaylistType type) => Find(type)?.Player.VolumeDb;
}

