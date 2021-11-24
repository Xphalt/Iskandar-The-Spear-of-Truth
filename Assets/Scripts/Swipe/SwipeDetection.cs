using System.Collections;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField]
    private float _minimumDistatance = .2f;
    [SerializeField, Range(.1f, 2f)]
    private float _maxiumtime = 1f;

    private PlayerInput _playerInput;
    private PlayerMovement_Jerzy _playerMovement;
    private GameObject _player;

    private Vector2 _startPosition;
    private float _startTime;

    private Vector2 _endPosition;
    private float _endTime;

    private void Awake()
    {
        _playerInput = FindObjectOfType<PlayerInput>().GetComponent<PlayerInput>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerMovement = FindObjectOfType<PlayerMovement_Jerzy>().GetComponent<PlayerMovement_Jerzy>();
    }

    private void OnEnable()
    {
        _playerInput.OnStartTouch += SwipeStart;
        _playerInput.OnEndTouch += SwipeEnd;
    }

    private void OnDisable()
    {
        _playerInput.OnStartTouch -= SwipeStart;
        _playerInput.OnEndTouch -= SwipeEnd;
    }

    private void SwipeStart(Vector2 position, float time)
    {
        _startPosition = position;
        _startTime = time;
    }

    private void SwipeEnd(Vector2 position, float time)
    {
        _endPosition = position;
        _endTime = time;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if (Vector3.Distance(_startPosition, _endPosition) >= _minimumDistatance && (_endTime - _startTime) <= _maxiumtime)
        {
            Vector2 direction2D = (_endPosition - _startPosition).normalized;
            SwipeDirection(direction2D);
        }
    }

    private void SwipeDirection(Vector2 direction)
    {
        Vector3 dashDirection = new Vector3(direction.x, _player.transform.position.y, direction.y);
        _playerMovement.Dash(dashDirection);
    }
}