﻿using Mutate4l.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mutate4l.Commands
{
    public enum TransposeMode
    {
        Absolute,
        Relative,
        Overwrite
    }

    public class TransposeOptions
    {
        public TransposeMode Mode { get; set; } = TransposeMode.Relative;

        public Clip By { get; set; } // Allows syntax like a1 transpose -by a2 -mode relative. This syntax makes it much clearer which clip is being affected, and which is used as the source.
    }

    public class Transpose
    {
        public static ProcessResultArray<Clip> Apply(TransposeOptions options, params Clip[] clips)
        {
            int basePitch = 60;
            if (options.Mode == TransposeMode.Relative && options.By.Notes.Count > 0)
            {
                basePitch = options.By.Notes[0].Pitch;
            }

            foreach (var clip in clips)
            {
                for (var i = 0; i < clip.Length; i++)
                {
                    var noteEvent = clip.Notes[i];
                    var transposeNoteEvent = options.By.Notes[i % options.By.Notes.Count];
                    if (options.Mode == TransposeMode.Overwrite)
                    {
                        noteEvent.Pitch = transposeNoteEvent.Pitch;
                    } else
                    {
                        noteEvent.Pitch += transposeNoteEvent.Pitch - basePitch;
                    }
                }
            }
            return new ProcessResultArray<Clip>(clips); // currently destructive
        }
    }
}
