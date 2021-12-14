using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UpdateEnemyHBar : MonoBehaviour
{
    public GameObject impactBar;
    public GameObject healthBar;
    public GameObject enemy;

    float healthDifference;
    public float impactTarget;
    public float multiplier;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.rotation = Quaternion.Euler(new Vector3(-transform.eulerAngles.x, 0, 0));

        healthBar.GetComponent<Image>().fillAmount = enemy.GetComponent<EnemyStats>().health/enemy.GetComponent<EnemyStats>().MAX_HEALTH;
        impactTarget = enemy.GetComponent<EnemyStats>().health;
        //impactBar.GetComponent<Image>().fillAmount = Mathf.Lerp(impactBar.GetComponent<Image>().fillAmount, impactTarget, multiplier);
        //multiplier *= 0.5f * Time.deltaTime;

        if (impactBar.GetComponent<Image>().fillAmount > healthBar.GetComponent<Image>().fillAmount)
        {
            healthDifference = impactBar.GetComponent<Image>().fillAmount - healthBar.GetComponent<Image>().fillAmount;
            healthDifference /= 20;
            impactBar.GetComponent<Image>().fillAmount -= healthDifference;
        }

        //impactBar.GetComponent<Image>().fillAmount = 0;

        float dist = Vector3.Distance(this.transform.position, GameObject.Find("Player").transform.position);

        if (dist < 5)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}