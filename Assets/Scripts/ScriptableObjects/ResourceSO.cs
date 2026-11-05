using UnityEngine;

[CreateAssetMenu(fileName = "ResourceSO", menuName = "Scriptable Objects/ResourceSO")]
public class ResourceSO : ScriptableObject {
    public string resourseName;
    public Sprite sprite;
    public Color color;
}
