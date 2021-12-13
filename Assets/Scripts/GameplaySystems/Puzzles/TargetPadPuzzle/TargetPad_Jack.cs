using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Detects and handles the player hitting the target pad
public class TargetPad_Jack : MonoBehaviour
{
    [SerializeField] private List<Animator> _attachedWalls;
    [SerializeField] private List<Light> _attachedLights;
    [SerializeField] private AnimationClip _wallAnimation;

    //public TogglePointLight toggleLightsScript;
    private Collider puzzleCollider;
    
    private string _playerSwordTag = "playerSword";
    private string _changeStateTrigger = "Change State";
    private float _timeSinceLastTriggered = 0.0f;

 //   public bool hasTriggered;   

    void Start()
    {
        _timeSinceLastTriggered = _wallAnimation.length;
        puzzleCollider = GetComponent<Collider>();

        _attachedLights[0].enabled = true;
        _attachedLights[1].enabled = false;
        _attachedLights[2].enabled = false;
        _attachedLights[3].enabled = true;
        _attachedLights[4].enabled = false;
        _attachedLights[5].enabled = true;
    }

	private void Update()
	{
        _timeSinceLastTriggered += Time.deltaTime;
    }

    // When hit by the player all attached walls will alternate between being risen & fallen
	private void OnTriggerEnter(Collider other)
	{
        if (other.attachedRigidbody.TryGetComponent(out ThrowSword_Jerzy sword)
            && _timeSinceLastTriggered > _wallAnimation.length)
        {
            if (!sword.PuzzleHit(puzzleCollider))
            {
                foreach (Animator wallAnimator in _attachedWalls)
                {
                    wallAnimator.SetTrigger(_changeStateTrigger);
                    _timeSinceLastTriggered = 0.0f;
                }
                foreach (Light pointLight in _attachedLights)
                    pointLight.enabled = !pointLight.enabled;
            }
        }
	}
}
