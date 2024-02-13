using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "Savepoint", menuName = "Savepoint")]
public class SO_Savepoint : ScriptableObject
{
    public string savepointID;
    public LocalizedString savepointName;
}
