using System;
using System.Collections.Generic;
using System.Reflection;
using GameFramework.Procedure;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Game.Hotfix.Framework
{
    public static class GameMain
    {
        public static void Start(object[] objects)
        {
            var hotfixAssemblies = (List<Assembly>)objects[0];

            GameEntryMain.Fsm.DestroyFsm<IProcedureManager>();

            var procedureManager = GameFramework.GameFrameworkEntry.GetModule<IProcedureManager>();
            var procedureTypes = GetProcedureTypes(hotfixAssemblies);
            Log.Info("Procedure count: {0}", procedureTypes.Count);
            var procedures = new ProcedureBase[procedureTypes.Count];
            for (var i = 0; i < procedureTypes.Count; i++)
            {
                procedures[i] = (ProcedureBase)Activator.CreateInstance(procedureTypes[i]);
                if (procedures[i] == null)
                {
                    Log.Error("Can not create procedure instance '{0}'.", procedureTypes[i].Name);
                    return;
                }
            }

            procedureManager.Initialize(GameFramework.GameFrameworkEntry.GetModule<GameFramework.Fsm.IFsmManager>(), procedures);
            procedureManager.StartProcedure<ProcedureInitFramework>();
        }

        private static List<Type> GetProcedureTypes(List<Assembly> assemblies)
        {
            var procedures = new List<Type>();
            var baseType = typeof(ProcedureBase);
            foreach (var hotfixAssembly in assemblies)
            {
                var types = hotfixAssembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.IsClass && !type.IsAbstract && baseType.IsAssignableFrom(type))
                    {
                        procedures.Add(type);
                    }
                }
            }
            return procedures;
        }
    }
}
