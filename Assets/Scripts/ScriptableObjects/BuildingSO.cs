using UnityEngine;

[CreateAssetMenu(fileName = "BuildingSO", menuName = "Scriptable Objects/BuildingSO")]
public class BuildingSO : ScriptableObject {
    public string buildingName;
    public Transform prefab;
    public Sprite sprite;
    public ResourceAmountData[] constructionResourceCostArray;
    public float minConstructionRadius = 7f;
    public float constructionTimerMax = 5f;
    public bool canBePlaced = true;

    public float healthAmountMax = 100f;

    public bool canGenerateResource = false;
    public ResourceGeneratorData resourceGeneratorData;

    public bool canDefend = false;
    public DefenseBuildingData defenseBuildingData;
}
