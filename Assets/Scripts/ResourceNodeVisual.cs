using UnityEngine;

public class ResourceNodeVisual : MonoBehaviour {
    [SerializeField] private SpriteRenderer[] spriteRendererArray;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material highlightMaterial;

    public void Highlight() {
        foreach (SpriteRenderer spriteRenderer in spriteRendererArray) {
            spriteRenderer.material = highlightMaterial;
        }
    }

    public void Unhighlight() {
        foreach (SpriteRenderer spriteRenderer in spriteRendererArray) {
            spriteRenderer.material = defaultMaterial;
        }
    }
}
