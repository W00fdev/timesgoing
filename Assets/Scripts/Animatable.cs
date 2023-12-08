using UnityEngine;

public class Animatable : MonoBehaviour
{
    [SerializeField] private string _idleTriggerName;
    [SerializeField] private string _runTriggerName;

    [SerializeField] private string _xValueName;
    [SerializeField] private string _yValueName;

    private int _idleHash;
    private int _runHash;
    private int _xValueHash;
    private int _yValueHash;

    private Animator _animator;

    // 0 - idle, 1 - running
    private bool _state;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();

        _idleHash = Animator.StringToHash(_idleTriggerName);
        _runHash = Animator.StringToHash(_runTriggerName);
        _xValueHash = Animator.StringToHash(_xValueName);
        _yValueHash = Animator.StringToHash(_yValueName);
    }

    public void ProcessInput(Vector3 unNormalizedInput)
    {
        if (unNormalizedInput.magnitude != 0 && _state == false)
        {
            _animator.SetTrigger(_runHash);
            _state = true;
        }
        else if (unNormalizedInput.magnitude == 0 && _state == true) 
        {
            _animator.SetTrigger(_idleHash);
            _state = false;
        }

        // Blending run
        _animator.SetFloat(_xValueHash, unNormalizedInput.x);
        _animator.SetFloat(_yValueHash, unNormalizedInput.z);
    }
}