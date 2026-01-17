using Godot;
using Godot.Collections;

namespace SteppeAttorney.Audio;

[GlobalClass]
public partial class Playlist : Resource
{
    [Export] public PlaylistType Type { get; set; }
    [Export] public Dictionary<string, Track> Tracks { get; set; } = [];
}
