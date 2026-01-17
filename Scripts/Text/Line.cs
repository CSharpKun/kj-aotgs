using Godot;

namespace SteppeAttorney.Text;

/// <summary>
/// Simple Line Resource that only contains text to process and animation's name
/// </summary>
[GlobalClass]
public partial class Line : Resource
{
    [Export] public string? Text { get; set; }
    [Export] public string? Animation { get; set; }
    [Export] public int Order { get; set; } = 0;
}
