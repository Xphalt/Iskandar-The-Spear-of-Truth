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

    // Start is called before the first frame update
    void Start()
    {
        _timeSinceLastTriggered = _wallAnimation.length;
    }

	private void Update()
	{
        _timeSinceLastTriggered += Time.deltaTime;
	}

    // When hit by the player all attached walls will alternate between being risen & fallen
	private void OnTriggerEnter(Collider other)
	{
        if (other.CompareTag(_playerSwordTag) 
            && _timeSinceLastTriggered > _wallAnimation.length)
        {
            foreach (Animator wallAnimator in _attachedWalls)
            {
                print("target hit");
                wallAnimator.SetTrigger(_changeStateTrigger);
                _timeSinceLastTriggered = 0.0f;
            }
        }
	}
}
