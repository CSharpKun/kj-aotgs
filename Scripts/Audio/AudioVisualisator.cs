using Godot;
using System;
using System.Collections.Generic;
using Analyzer = Godot.AudioEffectSpectrumAnalyzerInstance;

namespace SteppeAttorney.Audio;

public record MagnitudeParameters(int Definition, double LowFreq = 20, double HighFreq = 20000, double LowDB = -40, double MaxDB = 0);

public static class AudioVisualisator
{
    private record InternalMagnitudeParameters<T>(T List, Analyzer Spectrum, double Interval, double Freq, MagnitudeParameters FreqParameters) where T : IList<double>;

    public static Analyzer? GetSpectrumAnalyzer(StringName busName)
    {
        var id = AudioServer.GetBusIndex(busName);
        return AudioServer.GetBusEffectInstance(id, 0) as Analyzer;
    }

    public static TList GetFrequencyMagnitude<TList>(Analyzer analyzer, MagnitudeParameters? parameters = null) where TList : IList<double>, new()
    {
        TList enumerable = new();
        parameters ??= new(10);

        var freq = parameters.LowFreq;
        var interval = (parameters.HighFreq - parameters.LowFreq) / parameters.Definition;

        for (var i = 0; i < parameters.Definition; i++)
        {
            freq = AddHeightToList<TList>(new(enumerable, analyzer, interval, freq, parameters));
        }
        return enumerable;
    }
    private static double SimpleFrequencyMath(double freq, double freqLow, double freqHigh)
    {
        var result = (freq - freqLow) / (freqHigh - freqLow);
        result = Math.Pow(result, 4);
        return double.Lerp(freqLow, freqHigh, result);
    }

    private static double AddHeightToList<T>(InternalMagnitudeParameters<T> parameters) where T : IList<double>
    {
        var freqrangeLow = SimpleFrequencyMath(parameters.Freq, parameters.FreqParameters.LowFreq, parameters.FreqParameters.HighFreq);
        var freq = parameters.Freq + parameters.Interval;
        var freqrangeHigh = SimpleFrequencyMath(freq, parameters.FreqParameters.LowFreq, parameters.FreqParameters.HighFreq);

        Vector2 mag = parameters.Spectrum.GetMagnitudeForFrequencyRange((float)freqrangeLow, (float)freqrangeHigh);
        double result = Mathf.LinearToDb(mag.Length());

        result = (result - parameters.FreqParameters.LowDB) / (parameters.FreqParameters.MaxDB - parameters.FreqParameters.LowDB);
        result += 0.3 * (freq - parameters.FreqParameters.LowFreq) / (parameters.FreqParameters.HighFreq - parameters.FreqParameters.LowFreq);
        result = Math.Clamp(result, 0.05, 1.0);

        parameters.List.Add(result);
        return freq;
    }
}
