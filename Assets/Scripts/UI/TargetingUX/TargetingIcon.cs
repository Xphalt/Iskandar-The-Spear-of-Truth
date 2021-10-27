using UnityEngine;

public class TargetingIcon : MonoBehaviour
{
    [SerializeField] private Material indicatingMaterial;
    [SerializeField] private Material activeMaterial;

    public enum TARGETING_TYPE
    {
        INDICATING,
        ACTIVE,
        NUM_TARGETING_TYPES,
    }

    private Transform targetedTransform = null;
    private TARGETING_TYPE targetingType = TARGETING_TYPE.NUM_TARGETING_TYPES;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        // Update the icon's position to be over the targeted thing
        if (targetedTransform)
        {
            Vector3 new_pos = new Vector3(targetedTransform.position.x, targetedTransform.position.y + (1 * targetedTransform.localScale.y), targetedTransform.position.z);
            transform.position = new_pos;
        }
    }

    public void SetTarget(Transform target, TARGETING_TYPE type)
    {
        // Don't change the target icon from active to indicating, only the other way
        if ((targetingType == TARGETING_TYPE.NUM_TARGETING_TYPES) || (type == targetingType) || (type > TARGETING_TYPE.INDICATING))
        {
            targetedTransform = target;
            targetingType = type;

            switch (targetingType)
            {
                case TARGETING_TYPE.INDICATING:
                    meshRenderer.material = indicatingMaterial;
                    break;
                case TARGETING_TYPE.ACTIVE:
                    meshRenderer.material = activeMaterial;
                    break;
            }
        }
    }
}
