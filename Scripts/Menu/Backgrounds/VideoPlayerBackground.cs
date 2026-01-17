using Godot;
using SteppeAttorney.Audio;

namespace SteppeAttorney.Backgrounds;
public partial class VideoPlayerBackground : CanvasLayer
{
    [Export] private VideoStream Video { get; set; }
    [Export] private string MusicName { get; set; }
    [Export] private VideoStreamPlayer VideoPlayer { get; set; }

    private readonly AudioPlayer _player = AudioPlayer.Instance;

    public override void _Ready()
    {
        Layer = -1;
        VideoPlayer.Loop = true;
        var music = _player.GetTrackFromTechName(MusicName, PlaylistType.Music);
        if (music != null) _player.Play(music, PlaylistType.Music); // Used when background video doesn't have audio line.
        ReadVideo();
    }

    public void ReadVideo() 
    {
        VideoPlayer.Stop();
        VideoPlayer.Stream = Video;
        VideoPlayer.Play();
    }
}
