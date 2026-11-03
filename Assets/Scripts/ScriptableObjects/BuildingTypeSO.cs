using UnityEngine;

[CreateAssetMenu(fileName = "BuildingTypeSO", menuName = "Scriptable Objects/BuildingTypeSO")]
public class BuildingTypeSO : ScriptableObject {
    public string buildingName;
    public Transform prefab;
    public Sprite sprite;
    public ResourceGeneratorData resourceGeneratorData;
    public ResourceAmountData[] constructionResourceCostArray;
    public float minConstructionRadius = 7f;
    public bool canBePlaced = true;
    public float healthAmountMax = 100f;
}
