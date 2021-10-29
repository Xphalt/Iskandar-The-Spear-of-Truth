/*
 * Dominique Russell 29-10-2021
 * UIManager can enable the MoneyPopup and set the value it shows
 * A timer will automatically start when it's enabled so it closes after 5 seconds
 */
using UnityEngine;
using TMPro;

public class MoneyPopup : MonoBehaviour
{
    [SerializeField] private bool enableTimer = true;
    [SerializeField] private float disappearTime = 5;
    private float timer;

    private TextMeshProUGUI wealthValue;

    private void Awake()
    {
        timer = 0;
    }

    private void Start()
    {
        wealthValue = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (enableTimer && timer > disappearTime)
        {
            timer = 0;
            gameObject.SetActive(false);
        }
    }

    public void SetNumber(int wealth)
    {
        wealthValue = GetComponentInChildren<TextMeshProUGUI>();
        wealthValue.text = wealth.ToString();
    }
}
