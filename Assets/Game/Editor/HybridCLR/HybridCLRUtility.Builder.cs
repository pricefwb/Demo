using System;
using System.Collections.Generic;
using System.Linq;
using HybridCLR.Editor;
using HybridCLR.Editor.AOT;
using HybridCLR.Editor.Commands;
using HybridCLR.Editor.Meta;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public static partial class HybridCLRUtility
    {
        public static class Builder
        {
            [MenuItem("MyTools/HybridCLR/Build")]
            public static void Build()
            {
                Build(EditorUserBuildSettings.activeBuildTarget);
            }

            public static void Build(BuildTarget target)
            {
                CompileDllCommand.CompileDll(target);

                SyncSettings.SyncAll(target);

                CopyAssemblies.CopyHotfixAndAOTAssemblies(target);

                Debug.Log("HybridCLR Build Done.");
            }

        }
    }
}