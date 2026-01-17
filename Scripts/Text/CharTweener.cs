using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using SteppeAttorney.Data;

namespace SteppeAttorney.Text;

public static partial class CharTweener
{
    public struct TextTagPair(string text, string tag)
    {
        public string Text { get; set; } = text;
        public string Tag { get; set; } = tag;
    }

    private static readonly HashSet<string> no = [" ", "/", "|", "\\", "{", "}", "(", ")", ".", ",", "!", "?",
                           ":", ";", "\"", "'", "-", "_", "=", "+", "*", "&", "^", "%",
                           "$", "#", "@", "~", "`" ];

    public static List<TextTagPair> ParseTags(string text)
    {
        var parsedText = BBCodeRegex().Matches(text);
        List<TextTagPair> data = [];

        foreach (Match match in parsedText)
        {
            TextTagPair pair = new();
            if (match.Groups[1].Success) pair.Text = match.Groups[1].Value;
            if (match.Groups[2].Success) pair.Tag = match.Groups[2].Value;
            data.Add(pair);
        }

        return data;
    }


    public static async Task Tweener(string text, TextBox textBox, CancellationToken token, double delay = 250)
    {
        string[] fix = [.. text.ToCharArray().Select(sym => sym.ToString())];

        foreach (var sym in fix)
        {
            token.ThrowIfCancellationRequested();
            textBox.Label.AppendText(sym);
            if (!no.Contains(sym)) textBox.AudioPlayer.Play();
            await Task.Delay(TimeSpan.FromMilliseconds(delay), token);
        }
    }

    [GeneratedRegex(@"([^[\]]+)|(\[.*?\])")]
    private static partial Regex BBCodeRegex();
}
