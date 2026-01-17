using Godot;
using SteppeAttorney.Audio;
using System;

namespace SteppeAttorney.Menu;
public partial class ImmersiveMenu : Node
{
    [Export] private Menu? GUI { get; set; }
    [Export] private PackedScene? DefaultBackground { get; set; }
    private Node? _background;

    public override void _Ready()
    {
        PackedScene background = null; // GetSaveBasedBackground();
        UpdateBackground(background);
    }

    public PackedScene GetSaveBasedBackground()
    {
        throw new NotImplementedException();
    }

    public void UpdateBackground(PackedScene background)
    {
        _background?.QueueFree();
        var loadedBackground = background ?? DefaultBackground;
        _background = loadedBackground?.Instantiate();
        AddChild(_background);
    }

}
