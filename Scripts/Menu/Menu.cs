using Godot;
using Godot.Collections;
using System;

namespace SteppeAttorney.Menu;

public enum ButtonTypes
{
    Start,
    Options,
    Exit
}

public partial class Menu : Control 
{
    [Export] Dictionary<ButtonTypes, Button> Buttons { get; set; } = [];

    private readonly System.Collections.Generic.Dictionary<ButtonTypes, Action> _buttonActions;

    public Menu()
    {
        _buttonActions = new() {
            { ButtonTypes.Start, StartCallback },
            { ButtonTypes.Options, OptionsCallback },
            { ButtonTypes.Exit, ExitCallback }
        };
    }

    public override void _Ready()
    {
        

        foreach (var pair in Buttons)
        {
            pair.Value.Pressed += _buttonActions[pair.Key];
            if (pair.Key == ButtonTypes.Exit && OS.GetName() == "iOS")
            {
                pair.Value.QueueFree();
                Buttons.Remove(pair.Key);
            }
        } 
    }

    private void StartCallback()
    {

    }

    private void OptionsCallback()
    {

    }

    private void ExitCallback() 
    {
        GetTree().Quit();
    }
}
