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

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider>();
    }

	private void OnTriggerEnter(Collider other)
	{
		if(!_hasTriggered && other.CompareTag("Player"))
        {
            _hasTriggered = true;
            _collider.enabled = false;
		}
	}
}
