using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Detects and handles the player hitting the target pad
public class TargetPad_Jack : MonoBehaviour
{
    [SerializeField]
    private List<Animator> _attachedWalls;

    [SerializeField]
    private AnimationClip _wallAnimation;

    private string _playerSwordTag = "playerSword";
    private string _changeStateTrigger = "Change State";

    private float _timeSinceLastTriggered = 0.0f;

    private Collider puzzleCollider;

    // Start is called before the first frame update
    void Start()
    {
        _timeSinceLastTriggered = _wallAnimation.length;
        puzzleCollider = GetComponent<Collider>();
    }

	private void Update()
	{
        _timeSinceLastTriggered += Time.deltaTime;
	}

    // When hit by the player all attached walls will alternate between being risen & fallen
	private void OnTriggerEnter(Collider other)
	{
        if (other.attachedRigidbody.TryGetComponent(out ThrowSword_Jerzy sword) && _timeSinceLastTriggered > _wallAnimation.length)
        {
            if (!sword.PuzzleHit(puzzleCollider))
            {
                foreach (Animator wallAnimator in _attachedWalls)
                {
                    wallAnimator.SetTrigger(_changeStateTrigger);
                    _timeSinceLastTriggered = 0.0f;
                }
                sword.HitPuzzle(puzzleCollider);
            }
        }
	}
}
