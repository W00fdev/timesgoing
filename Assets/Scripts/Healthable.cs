using UnityEngine.Events;
using System.Collections;
using UnityEngine;
using System;

public class Healthable : MonoBehaviour
{
    public enum OnDeadActionType { Destroy, Deactivate, Action, Event };

    [SerializeField]
    private int _maxHP;
    [SerializeField]
    private int _hp;

    [SerializeField]
    private OnDeadActionType _onDead;

    public UnityEvent OnDeadEvent;
    public Action<Healthable> OnDeadAction;

    public void Damage(int damage)
    {
        _hp -= damage;

        if (_hp <= 0)
            Death();
    }

    public void Death()
    {
        switch (_onDead)
        {
            case OnDeadActionType.Destroy:
                Destroy(gameObject);
                break;
            case OnDeadActionType.Deactivate:
                gameObject.SetActive(false);
                break;
            case OnDeadActionType.Action:
                OnDeadAction?.Invoke(this);
                break;
            case OnDeadActionType.Event:
                OnDeadEvent.Invoke();
                break;
        }
    }

    public void Death(float time)
    {
        switch (_onDead)
        {
            case OnDeadActionType.Destroy:
                StartCoroutine(DelayedAction(() => Destroy(gameObject), time));
                break;
            case OnDeadActionType.Deactivate:
                StartCoroutine(DelayedAction(() => gameObject.SetActive(false), time));
                break;
            case OnDeadActionType.Action:
                StartCoroutine(DelayedAction(() => OnDeadAction?.Invoke(this), time));
                break;
            case OnDeadActionType.Event:
                StartCoroutine(DelayedAction(() => OnDeadEvent.Invoke(), time));
                break;
        }
    }

    IEnumerator DelayedAction(Action delayed, float time)
    {
        yield return new WaitForSeconds(time);
        delayed.Invoke();
    }
}