﻿using Hai.ComboGesture.Scripts.Components;
using UnityEditor.Animations;
using UnityEngine;

namespace Hai.ComboGesture.Scripts.Editor.Internal
{
    internal class SharedLayerUtils
    {
        internal static void SetupImmediateTransition(AnimatorStateTransition transition)
        {
            SetupCommonTransition(transition);

            transition.duration = 0;
            transition.orderedInterruption = true;
            transition.canTransitionToSelf = false;
        }

        internal static void SetupDefaultTransition(AnimatorStateTransition transition)
        {
            SetupCommonTransition(transition);

            transition.duration = 0.1f; // There seems to be a quirk if the duration is 0 when using DisableExpressions, so use 0.1f instead
            transition.orderedInterruption = true;
            transition.canTransitionToSelf = true; // This is relevant as normal transitions may not check activity nor disabled expressions
        }

        internal static void SetupDefaultBlinkingTransition(AnimatorStateTransition transition)
        {
            SetupCommonTransition(transition);

            transition.duration = 0;
            transition.orderedInterruption = false; // Is the difference relevant?!
            transition.canTransitionToSelf = false;
        }

        internal static void SetupCommonTransition(AnimatorStateTransition transition)
        {
            transition.hasExitTime = false;
            transition.exitTime = 0;
            transition.hasFixedDuration = true;
            transition.offset = 0;
            transition.interruptionSource = TransitionInterruptionSource.None;
        }

        internal static Vector3 GridPosition(int x, int y)
        {
            return new Vector3(x * 200 , y * 70, 0);
        }

        internal const string HaiGestureComboParamName = "_Hai_GestureComboValue";
        internal const string HaiGestureComboDisableExpressionsParamName = "_Hai_GestureComboDisableExpressions";
        internal const string HaiGestureComboAreEyesClosed = "_Hai_GestureComboAreEyesClosed";
        internal const string HaiGestureComboDisableBlinkingOverrideParamName = "_Hai_GestureComboDisableBlinkingOverride";
        internal const string HaiGestureComboIsLipsyncLimited = "_Hai_GestureComboIsLipsyncLimited";
        internal const string HaiGestureComboDisableLipsyncOverrideParamName = "_Hai_GestureComboDisableLipsyncOverride";

        public static RawGestureManifest FromManifest(ComboGestureActivity activity, AnimationClip fallbackWhen00ClipIsNull)
        {
            return activity == null
                ? RawGestureManifest.AllSlotsFittedWithSameClip(fallbackWhen00ClipIsNull)
                : RawGestureManifest.FromActivity(activity, fallbackWhen00ClipIsNull);
        }

    }
}