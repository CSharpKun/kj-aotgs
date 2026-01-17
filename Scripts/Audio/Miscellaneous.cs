using Godot;

namespace SteppeAttorney.Audio;

public enum PlaylistType
{
    Music = 0,
    Sound,
    Ambient,
    Voice
}

public static class AudioExtensions
{
    public static Track GetTrackByKey(this Godot.Collections.Dictionary<string, Track> dict, string key)
    {
        if (dict.TryGetValue(key, out Track track)) return track;

        GD.PrintErr($"Track with key {key} not found in this Playlist");
        return null;
    }

    public static Track GetTrackByStream(this Godot.Collections.Dictionary<string, Track> dict, AudioStream stream)
    {
        foreach (var pair in dict)
        {
            if (pair.Value.Audio == stream) return pair.Value;
        }
        GD.PrintErr($"AudioStream {stream.ResourceName} and it's associated Track were not found in this Playlist");
        return null;
    }

    public static string GetKeyByTrack(this Godot.Collections.Dictionary<string, Track> dict, Track track)
    {
        foreach (var pair in dict)
        {
            if (pair.Value == track) return pair.Key;
        }
        GD.PrintErr($"Track {track.ResourceName} and it's associated key were not found in this Playlist");
        return null;
    }

    public static string GetKeyByStream(this Godot.Collections.Dictionary<string, Track> dict, AudioStream stream)
    {
        foreach (var pair in dict)
        {
            if (pair.Value.Audio == stream) return pair.Key;
        }
        GD.PrintErr($"AudioStream {stream.ResourceName} and it's associated key were not found in this Playlist");
        return null;
    }
}
