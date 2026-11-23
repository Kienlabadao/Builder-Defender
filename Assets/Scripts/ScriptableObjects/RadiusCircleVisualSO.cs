using UnityEngine;

[CreateAssetMenu(fileName = "RadiusCircleVisualSO", menuName = "Scriptable Objects/RadiusCircleVisualSO")]
public class RadiusCircleVisualSO : ScriptableObject {
    public RadiusCircleType radiusCircleType;
    public Sprite sprite;
    public Color color;
    public bool useSpinAnimation = false;
}
