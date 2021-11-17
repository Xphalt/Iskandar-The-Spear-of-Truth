/*
 * Dominique 28-10-2021
 * This script should be on a UI targeting icon image
 * It can be updated to change colour according to if it is on an active target or indicating a targetable object
 * It can also be updated to have a different sprite according to the active input
 */

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetingIcon : MonoBehaviour
{
    [SerializeField] private Sprite gamepadSprite;
    [SerializeField] private Sprite keyboardSprite;
    [SerializeField] private Sprite mobileSprite;

    [SerializeField] private Color indicatingColor;
    [SerializeField] private Color activeColor;

    private TextMeshProUGUI keyIndicator;

    public enum TARGETING_TYPE
    {
        INDICATING,
        ACTIVE,
        NUM_TARGETING_TYPES,
    }

    private Transform targetedTransform = null;
    private EnemyStats targetedStats = null;
    private TARGETING_TYPE targetingType = TARGETING_TYPE.NUM_TARGETING_TYPES;
    private Image iconImage;

    private void Awake()
    {
        iconImage = GetComponent<Image>();
        keyIndicator = GetComponentInChildren<TextMeshProUGUI>();
        gameObject.SetActive(false);
    }

    private void Start()
    {
        SetSprite(UIManager.instance.GetCurrentInput());
    }

    private void Update()
    {
        // Update the icon's position to be over the targeted thing
        if (targetedTransform)
        {
            Vector3 new_pos = new Vector3(targetedTransform.position.x, targetedTransform.position.y + (3 * targetedTransform.localScale.y), targetedTransform.position.z);
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(new_pos);
            transform.position = screenPoint;

            // If the enemy dies disable the icon
            if (targetedStats.IsDead())
            {
                SetTarget(null, TARGETING_TYPE.NUM_TARGETING_TYPES);
            }
        }
    }

    // Set the transform we're tracking along with if this is just a target indicator or we're actively targeting
    public void SetTarget(Transform target, TARGETING_TYPE type)
    {
        // Don't change the target icon from active to indicating, only the other way
        if ((targetingType == TARGETING_TYPE.NUM_TARGETING_TYPES) || (type == targetingType) || (type > TARGETING_TYPE.INDICATING))
        {
            targetedTransform = target;
            if (targetedTransform)
            {
                targetedStats = targetedTransform.GetComponent<EnemyStats>();
            }
            else
            {
                targetedStats = null;
            }
            targetingType = type;

            switch (targetingType)
            {
                case TARGETING_TYPE.INDICATING:
                    iconImage.color = Color.white;
                    break;
                case TARGETING_TYPE.ACTIVE:
                    iconImage.color = Color.red;
                    break;
            }
        }
    }

    // Different sprites according to UI input
    public void SetSprite(UIManager.INPUT_OPTIONS input_option)
    {
        switch (input_option)
        {
            case UIManager.INPUT_OPTIONS.GAMEPAD:
                iconImage.sprite = gamepadSprite;
                keyIndicator.gameObject.SetActive(false);
                break;
            case UIManager.INPUT_OPTIONS.KEYBOAD_AND_MOUSE:
                iconImage.sprite = keyboardSprite;
                keyIndicator.gameObject.SetActive(true);
                break;
            case UIManager.INPUT_OPTIONS.MOBILE:
                iconImage.sprite = mobileSprite;
                keyIndicator.gameObject.SetActive(false);
                break;
        }
    }
}
