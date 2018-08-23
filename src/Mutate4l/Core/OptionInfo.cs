﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Mutate4l.Core
{
    public enum OptionType
    {
        AllOrSpecified, // If none specified, all are active. Otherwise, only specified options are active.
        Value, // Option that takes one or more values.
        Default // If values are given without an option header, use them for this parameter.
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class OptionInfo : Attribute
    {
        public int GroupId { get; }
        public OptionType Type { get; }
        public int? MinNumberValue { get; }
        public int? MaxNumberValue { get; }

        public OptionInfo(int groupId, OptionType type)
        {
            GroupId = groupId;
            Type = type;
        }

        public OptionInfo(OptionType type) : this(0, type) { }

        public OptionInfo(int min, int max) : this(0, OptionType.Value)
        {
            MinNumberValue = min;
            MaxNumberValue = max;
        }
    }
}
