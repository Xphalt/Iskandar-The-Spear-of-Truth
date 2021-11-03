using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//morgan S Script

public class ScrDoor1 : MonoBehaviour
{
    public int id;

    private bool _locked = false;
    public bool Locked
    {
        get => _locked;
        set { _locked = value; }
	}

    private Animator _animator;

    [SerializeField] private bool _startsOpen = false;

    private float timer = 0;

    // Alternate the door between open and closed if it's unlocked
    public void SwapDoor()
    {
        if(!_locked) _animator.SetTrigger("Swap");
    }

    private void Start()
    {
        GameEvents.current.onDoorwayTriggerEnter += OnDoorwayOpen;
        GameEvents.current.onDoorwayTriggerExit += OnDoorwayClose;

        _animator = GetComponent<Animator>();
        _animator.SetBool("StartsOpen", _startsOpen);
    }

	private void OnDoorwayOpen(int id)
    {
        if (id == this.id)
        {
            SwapDoor();
        }
    }

    private void OnDoorwayClose(int id)
    {
        if (id == this.id)
        {
            SwapDoor();
        }
    }

    private void OnDestroy()
    {
        GameEvents.current.onDoorwayTriggerEnter -= OnDoorwayOpen;
        GameEvents.current.onDoorwayTriggerExit -= OnDoorwayClose;
    }
}
