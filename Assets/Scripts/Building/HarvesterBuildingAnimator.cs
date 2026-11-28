using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HarvesterBuildingAnimator : MonoBehaviour {
    private const string IS_WORKING = "IsWorking";

    [SerializeField] private ResourceGenerator resourceGenerator;

    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();

        resourceGenerator.OnInitializeCompleted += ResourceGenerator_OnInitializeCompleted;
        resourceGenerator.OnGeneratorStopped += ResourceGenerator_OnGeneratorStopped;
    }

    private void OnDestroy() {
        resourceGenerator.OnInitializeCompleted -= ResourceGenerator_OnInitializeCompleted;
        resourceGenerator.OnGeneratorStopped -= ResourceGenerator_OnGeneratorStopped;
    }

    private void ResourceGenerator_OnGeneratorStopped(object sender, System.EventArgs e) {
        animator.SetBool(IS_WORKING, false);
    }

    private void ResourceGenerator_OnInitializeCompleted(object sender, System.EventArgs e) {
        animator.SetBool(IS_WORKING, resourceGenerator.IsEnabled());
    }
}
