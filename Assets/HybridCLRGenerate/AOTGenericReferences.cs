using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"GameFramework.dll",
		"UnityEngine.CoreModule.dll",
		"UnityGameFramework.Runtime.dll",
		"mscorlib.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// System.Collections.Generic.ArraySortHelper<byte>
	// System.Collections.Generic.ArraySortHelper<object>
	// System.Collections.Generic.Comparer<byte>
	// System.Collections.Generic.Comparer<object>
	// System.Collections.Generic.ICollection<byte>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.IComparer<byte>
	// System.Collections.Generic.IComparer<object>
	// System.Collections.Generic.IEnumerable<byte>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerator<byte>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.List.Enumerator<byte>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List<byte>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.ObjectComparer<byte>
	// System.Collections.Generic.ObjectComparer<object>
	// System.Comparison<byte>
	// System.Comparison<object>
	// System.Predicate<byte>
	// System.Predicate<object>
	// }}

	public void RefMethods()
	{
		// object GameFramework.DataNode.IDataNodeManager.GetData<object>(string)
		// object UnityEngine.Object.Instantiate<object>(object)
		// object UnityGameFramework.Runtime.DataNodeComponent.GetData<object>(string)
	}
}