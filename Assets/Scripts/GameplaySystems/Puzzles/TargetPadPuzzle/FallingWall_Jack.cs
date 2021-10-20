using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingWall_Jack : MonoBehaviour
{
    [SerializeField]
    private bool _startsDown = false;

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool("Starts Down", _startsDown);
    }
}
