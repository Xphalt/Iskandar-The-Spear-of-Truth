using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Detects and handles the player hitting the target pad
public class TargetPad_Jack : MonoBehaviour
{
    [SerializeField] private List<Animator> _attachedWalls;
    [SerializeField] private AnimationClip _wallAnimation;

    private Light pointLight;
    private Collider puzzleCollider;
    
    private string _playerSwordTag = "playerSword";
    private string _changeStateTrigger = "Change State";
    private float _timeSinceLastTriggered = 0.0f;

    public bool hasTriggered;   

    void Start()
    {
        _timeSinceLastTriggered = _wallAnimation.length;
        pointLight = GetComponent<Light>();
        puzzleCollider = GetComponent<Collider>();
    }

	private void Update()
	{
        _timeSinceLastTriggered += Time.deltaTime;

        if (hasTriggered)
        {
            pointLight.enabled = true; 
         
        }
        else pointLight.enabled = false;

        print("triggered: " + pointLight.enabled);
    }


    // When hit by the player all attached walls will alternate between being risen & fallen
	private void OnTriggerEnter(Collider other)
	{
        if (other.attachedRigidbody.TryGetComponent(out ThrowSword_Jerzy sword)
            && _timeSinceLastTriggered > _wallAnimation.length)
        {
            if (!sword.PuzzleHit(puzzleCollider))
            {
                hasTriggered = hasTriggered ? false : true;

                foreach (Animator wallAnimator in _attachedWalls)
                {
                    wallAnimator.SetTrigger(_changeStateTrigger);
                    _timeSinceLastTriggered = 0.0f;
                }
            }
        }
	}
}
