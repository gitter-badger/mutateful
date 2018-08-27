﻿using Mutate4l.Core;

namespace Mutate4l.Options
{
    public enum Shape
    {
        Linear,
        EaseInOut,
        EaseIn
    }

    public enum RatchetMode
    {
        // whether ratcheting should be applied based on velocity or pitch in the control clip
        Velocity,
        Pitch
    }

    public class RatchetOptions
    {
        public RatchetMode Mode { get; set; } = RatchetMode.Velocity;

        [OptionInfo(min: 1, max: 20)]
        public int Min { get; set; } = 1;

        [OptionInfo(min: 1, max: 20)]
        public int Max { get; set; } = 8;

        [OptionInfo(min: 0, max: 100)]
        public int Strength { get; set; } = 100;

        public bool VelocityToStrength { get; set; }

        public Shape Shape { get; set; } = Shape.Linear;

        // Automatically scale control sequence so that lowest note corresponds to minimum ratchet value and highest note corresponds to maximum ratchet value
        public bool AutoScale { get; set; }

        public int ControlMin { get; set; } = 60; // default lowest pitch for control sequence (unless AutoScale is on), e.g. pitch values 60 or lower equal Min ratchet-value.

        public int ControlMax { get; set; } = 68; // default highest pitch for control sequence (unless AutoScale is on), e.g. pitch values 72 or higher equal Max ratchet-value.
    }
}