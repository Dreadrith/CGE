﻿using System;
using Hai.ComboGesture.Scripts.Editor.Internal.CgeAac;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Hai.ComboGesture.Scripts.Editor.Internal
{
    internal class AssetContainer
    {
        private readonly AnimatorController _holder;
        private readonly AacFlBase _aac;

        private AssetContainer(AnimatorController holder)
        {
            _holder = holder;

            _aac = AacV0.Create(new AacConfiguration
            {
                AnimatorRoot = null,
                AssetContainer = _holder,
                AssetKey = "GeneratedCGE",
                AvatarDescriptor = null,
                DefaultsProvider = new CgeDefaultsProvider(true),
                SystemName = "CGE",
                DefaultValueRoot = null
            });
        }

        public static AssetContainer CreateNew(string folderToCreateAssetIn)
        {
            var holder = new AnimatorController();
            var container = new AssetContainer(holder);
            AssetDatabase.CreateAsset(holder, folderToCreateAssetIn + "/GeneratedCGE__" + DateTime.Now.ToString("yyyy'-'MM'-'dd'_'HHmmss") + ".asset");
            return container;
        }

        public static AssetContainer FromExisting(RuntimeAnimatorController existingContainer)
        {
            var assetContainer = (AnimatorController) existingContainer;
            if (assetContainer == null)
            {
                throw new ArgumentException("An asset container must not be null.");
            }
            if (assetContainer.layers.Length != 0)
            {
                throw new ArgumentException("An asset container must have 0 layers to be safely used.");
            }
            if (!AssetDatabase.Contains(assetContainer))
            {
                throw new ArgumentException("The asset container must already be an asset");
            }

            return new AssetContainer(assetContainer);
        }

        public void AddAnimation(AnimationClip animation)
        {
            _aac.CGE_StoringMotion(animation);
        }

        public void AddBlendTree(BlendTree blendTree)
        {
            _aac.CGE_StoringMotion(blendTree);
        }

        public void AddAvatarMask(AvatarMask mask)
        {
            _aac.CGE_StoringAsset(mask);
        }

        public RuntimeAnimatorController ExposeContainerAsset()
        {
            return _holder;
        }

        public static void GlobalSave()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public AacFlBase ExposeAac()
        {
            return _aac;
        }
    }

    public class CgeDefaultsProvider : IAacDefaultsProvider
    {
        private readonly bool _writeDefaults;

        public CgeDefaultsProvider(bool writeDefaults = false)
        {
            _writeDefaults = writeDefaults;
        }

        public virtual void ConfigureState(AnimatorState state, AnimationClip emptyClip)
        {
            state.motion = emptyClip;
            state.writeDefaultValues = _writeDefaults;
        }

        public virtual void ConfigureTransition(AnimatorStateTransition transition)
        {
            transition.duration = 0;
            transition.hasExitTime = false;
            transition.exitTime = 0;
            transition.hasFixedDuration = true;
            transition.offset = 0;
            transition.interruptionSource = TransitionInterruptionSource.None;
            transition.orderedInterruption = true;
            transition.canTransitionToSelf = false;
        }

        public virtual string ConvertLayerName(string systemName)
        {
            return systemName;
        }

        public virtual string ConvertLayerNameWithSuffix(string systemName, string suffix)
        {
            return suffix;
        }

        public Vector2 Grid()
        {
            return new Vector2(250, 70);
        }

        public void ConfigureStateMachine(AnimatorStateMachine stateMachine)
        {
            var grid = Grid();
            stateMachine.anyStatePosition = grid * new Vector2(0, 7);
            stateMachine.entryPosition = grid * new Vector2(0, -1);
            stateMachine.exitPosition = grid * new Vector2(7, -1);
            stateMachine.parentStateMachinePosition = grid * new Vector2(3, -1);
        }
    }
}
