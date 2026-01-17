using Godot;

namespace SteppeAttorney.Audio;

[GlobalClass]
public partial class Track : Resource
{
    [Export] public AudioStream Audio { get; set; }
    [Export] public string Name { get; set; }
}
