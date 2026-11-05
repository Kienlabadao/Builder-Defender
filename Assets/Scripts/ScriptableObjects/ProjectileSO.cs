using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSO", menuName = "Scriptable Objects/ProjectileSO")]
public class ProjectileSO : ScriptableObject {
    public string projectileName;
    public Transform prefab;
    public float projectileSpeed = 20f;
}
