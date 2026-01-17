using Godot;
using Godot.Collections;

namespace SteppeAttorney.Text;

/// <summary>
/// Main Dialogue resource
/// </summary>
[GlobalClass]
public partial class Dialogue : Resource
{
    [Export] public string? Name { get; set; }
    [Export] public Array<LineSet>? LineSets { get; set; }
}