using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class RadiusCircle : MonoBehaviour {
    [SerializeField] private RadiusCircleVisualListSO radiusCircleVisualListSO;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Dictionary<RadiusCircleType, RadiusCircleVisualSO> radiusCircleVisualSODictionary;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        radiusCircleVisualSODictionary = radiusCircleVisualListSO
            .radiusCircleVisualSOArray
            .ToDictionary(radiusCircleVisualSO => radiusCircleVisualSO.radiusCircleType);
    }

    private float GetSpriteDiameter() {
        return spriteRenderer.sprite.bounds.size.x;
    }

    public void Show(RadiusCircleType radiusCircleType, float radius) {
        float diameter = radius * 2f;
        float scale = (diameter) / GetSpriteDiameter();

        transform.localScale = Vector3.one * scale;

        if (radiusCircleVisualSODictionary.TryGetValue(radiusCircleType, out RadiusCircleVisualSO radiusCircleVisualSO)) {
            spriteRenderer.sprite = radiusCircleVisualSO.sprite;
            spriteRenderer.color = radiusCircleVisualSO.color;

            animator.enabled = radiusCircleVisualSO.useSpinAnimation;
        } else {
            Debug.LogWarning($"{radiusCircleVisualListSO} doesn't contain entry with key {radiusCircleType}!");
        }

        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
