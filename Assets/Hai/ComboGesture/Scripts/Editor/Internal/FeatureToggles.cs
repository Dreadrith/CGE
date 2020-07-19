﻿using System;

namespace Hai.ComboGesture.Scripts.Editor.Internal
{
    [Flags]
    public enum FeatureToggles
    {
        None = 0,
        ExposeDisableExpressions = 1,
        ExposeDisableBlinkingOverride = 2,
        ExposeAreEyesClosed = 4
    }
}