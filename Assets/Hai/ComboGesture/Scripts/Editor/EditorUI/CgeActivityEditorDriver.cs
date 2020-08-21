﻿using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Hai.ComboGesture.Scripts.Editor.EditorUI
{
    public class CgeActivityEditorDriver
    {
        private static readonly Dictionary<string, string> Translations = new Dictionary<string, string>
        {
            {"anim00", "No gesture"},
            {"anim01", "FIST"},
            {"anim02", "OPEN"},
            {"anim03", "POINT"},
            {"anim04", "PEACE"},
            {"anim05", "ROCKNROLL"},
            {"anim06", "GUN"},
            {"anim07", "THUMBSUP"},
            {"anim11", "FIST x2, L+R trigger"},
            {"anim11_L", "FIST x2, LEFT trigger"},
            {"anim11_R", "FIST x2, RIGHT trigger"},
            {"anim12", "OPEN + FIST"},
            {"anim13", "POINT + FIST"},
            {"anim14", "PEACE + FIST"},
            {"anim15", "ROCKNROLL + FIST"},
            {"anim16", "GUN + FIST"},
            {"anim17", "THUMBSUP + FIST"},
            {"anim22", "OPEN x2"},
            {"anim23", "OPEN + POINT"},
            {"anim24", "OPEN + PEACE"},
            {"anim25", "OPEN + ROCKNROLL"},
            {"anim26", "OPEN + GUN"},
            {"anim27", "OPEN + THUMBSUP"},
            {"anim33", "POINT x2"},
            {"anim34", "POINT + PEACE"},
            {"anim35", "POINT + ROCKNROLL"},
            {"anim36", "POINT + GUN"},
            {"anim37", "POINT + THUMBSUP"},
            {"anim44", "PEACE x2"},
            {"anim45", "PEACE + ROCKNROLL"},
            {"anim46", "PEACE + GUN"},
            {"anim47", "PEACE + THUMBSUP"},
            {"anim55", "ROCKNROLL x2"},
            {"anim56", "ROCKNROLL + GUN"},
            {"anim57", "ROCKNROLL + THUMBSUP"},
            {"anim66", "GUN x2"},
            {"anim67", "GUN + THUMBSUP"},
            {"anim77", "THUMBSUP x2"}
        };

        private static readonly Dictionary<string, MergePair> ParameterToMerge = new Dictionary<string, MergePair>
        {
            {"anim12", new MergePair("anim01", "anim02")},
            {"anim13", new MergePair("anim01", "anim03")},
            {"anim14", new MergePair("anim01", "anim04")},
            {"anim15", new MergePair("anim01", "anim05")},
            {"anim16", new MergePair("anim01", "anim06")},
            {"anim17", new MergePair("anim01", "anim07")},
            {"anim23", new MergePair("anim02", "anim03")},
            {"anim24", new MergePair("anim02", "anim04")},
            {"anim25", new MergePair("anim02", "anim05")},
            {"anim26", new MergePair("anim02", "anim06")},
            {"anim27", new MergePair("anim02", "anim07")},
            {"anim34", new MergePair("anim03", "anim04")},
            {"anim35", new MergePair("anim03", "anim05")},
            {"anim36", new MergePair("anim03", "anim06")},
            {"anim37", new MergePair("anim03", "anim07")},
            {"anim45", new MergePair("anim04", "anim05")},
            {"anim46", new MergePair("anim04", "anim06")},
            {"anim47", new MergePair("anim04", "anim07")},
            {"anim56", new MergePair("anim05", "anim06")},
            {"anim57", new MergePair("anim05", "anim07")},
            {"anim67", new MergePair("anim06", "anim07")}
        };
        private static readonly Dictionary<string, string> AnimationToCopy = new Dictionary<string, string>
        {
            {"anim11", "anim01"},
            {"anim12", "anim02"},
            {"anim13", "anim03"},
            {"anim14", "anim04"},
            {"anim15", "anim05"},
            {"anim16", "anim06"},
            {"anim17", "anim07"},
            {"anim22", "anim02"},
            {"anim33", "anim03"},
            {"anim44", "anim04"},
            {"anim55", "anim05"},
            {"anim66", "anim06"},
            {"anim77", "anim07"}
        };

        public bool IsSymmetrical(string propertyPath)
        {
            return Translations[propertyPath].Contains("x2");
        }

        public string ShortTranslation(string propertyPath)
        {
            return Translations[propertyPath];
        }

        public bool IsAPropertyThatCanBeCombined(string propertyPath)
        {
            return ParameterToMerge.ContainsKey(propertyPath);
        }

        public bool AreCombinationSourcesDefinedAndCompatible(SerializedObject serializedObject, string propertyPath)
        {
            if (!IsAPropertyThatCanBeCombined(propertyPath))
            {
                return false;
            }

            var mergePair = ParameterToMerge[propertyPath];
            var left = serializedObject.FindProperty(mergePair.Left).objectReferenceValue;
            var right = serializedObject.FindProperty(mergePair.Right).objectReferenceValue;

            Debug.Log(propertyPath + " has " + left + " " + right);

            return left != null && right != null && left != right;
        }

        public MergePair ProvideCombinationPropertySources(string propertyPath)
        {
            if (!IsAPropertyThatCanBeCombined(propertyPath))
            {
                throw new ArgumentException();
            }

            return ParameterToMerge[propertyPath];
        }

        public bool IsAutoSettable(string propertyPath)
        {
            return AnimationToCopy.ContainsKey(propertyPath);
        }

        public string GetAutoSettableSource(string propertyPath)
        {
            return AnimationToCopy[propertyPath];
        }
    }

    public readonly struct MergePair
    {
        public MergePair(string left, string right)
        {
            Left = left;
            Right = right;
        }

        internal string Left { get; }
        internal string Right { get; }
    }
}