using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A trigger that only triggers once
public class OneTimeTrigger : MonoBehaviour
{
    private bool _hasTriggered = false;
    public bool HasTriggered
    {
        get => _hasTriggered;
	}

    private Collider _collider;
    public Collider Collider
    {
        get => _collider;
    }

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider>();
    }

	private void OnTriggerEnter(Collider other)
	{
        if (this.gameObject.name.Contains("Trigger (") && !_hasTriggered && other.CompareTag("playerSword"))
        {
            PlayerCombat_Jerzy combat = FindObjectOfType<PlayerCombat_Jerzy>();
            if (!combat.attackOffCooldown || !combat.swordObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Simple Attack"))
            {
                _hasTriggered = true;
                _collider.enabled = false;
            }
            _hasTriggered = true;
            _collider.enabled = false;
        }
        else if (!this.gameObject.name.Contains("Trigger (") && !_hasTriggered && other.CompareTag("Player"))
        {
            _hasTriggered = true;
            _collider.enabled = false;
		}
	}
} 