using Godot;

namespace SteppeAttorney.Data;

public struct TextBox
{
    public RichTextLabel Label { get; set; }
    public RichTextLabel NameTag { get; set; }
    public AudioStreamPlayer AudioPlayer { get; set; }
}