using Godot;
using Godot.Collections;

namespace SteppeAttorney.Menu;

public enum OptionsButtons
{
    Base,
    Sound,
    Graphic,
    Exit
}

public partial class Options : Control
{
    [Export] private Dictionary<OptionsButtons, BaseButton> Buttons { get; set; } = [];
}
