using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damnation : MonoBehaviour
{
    public Transform[] rows;

    public float spikeInterval, scaleIncrease, spikeLifetime;
    public Vector3 defaultScale = Vector3.one;
    private int spawnIndex = 0, despawnIndex = 0;
    private float spikeGap, spawnTimer = 0, lifeTimer = 0;
    private bool casting = false;

    private Transform parent;
    private Vector3 startScale;

    public float RowAngle => 360 / rows.Length;
    public bool Finished => !casting;

    private float _maxRange = 0;
    public float MaxRange => _maxRange;

    private void Start()
    {
        foreach (Transform row in transform.GetComponent<Transform>())
        {
            foreach (Transform spike in row.GetComponentInChildren<Transform>()) spike.gameObject.SetActive(false);
            row.gameObject.SetActive(false);
        }

        for (int i = 0; i < rows[0].childCount; i++) _maxRange += spikeGap + scaleIncrease * i;

        startScale = transform.localScale;
        parent = transform.parent;
    }

    private void Update()
    {
        if (casting)
        {
            spawnTimer += Time.deltaTime;
            lifeTimer += Time.deltaTime;

            if (spawnTimer > spikeInterval)
            {
                foreach (Transform row in rows)
                {
                    if (spawnIndex >= row.childCount) break;

                    Transform newSpike = row.GetChild(spawnIndex);
                    newSpike.position = transform.position + spawnIndex * (spikeGap + scaleIncrease * spawnIndex) * row.forward;
                    newSpike.localScale = defaultScale * (1 + (scaleIncrease * spawnIndex));

                    newSpike.gameObject.SetActive(true);
                }

                if (lifeTimer > spikeLifetime)
                {
                    foreach (Transform row in rows)
                    {
                        if (despawnIndex == row.childCount)
                        {
                            casting = false;
                            row.gameObject.SetActive(false);
                        }
                        else row.GetChild(despawnIndex).gameObject.SetActive(false);
                    }
                    despawnIndex++;
                }

                spawnTimer = 0;
                spawnIndex++;
            }
        }
    }

    public void Cast(float rotation, float range)
    {
        for (int r = 0; r < rows.Length; r++)
        {
            rows[r].rotation = Quaternion.Euler(Vector3.up * (rotation + RowAngle * r));
            rows[r].gameObject.SetActive(true);
        }
        spawnIndex = 0;
        spawnTimer = spikeInterval;
        despawnIndex = 0;
        lifeTimer = 0;
        casting = true;
        transform.localPosition = Vector3.zero;
        transform.SetParent(null);

        spikeGap = (range + (scaleIncrease * rows[0].childCount)) / rows[0].childCount;
    }

    public void EndCast()
    {
        transform.SetParent(parent);
        transform.localScale = startScale;
    }
}
