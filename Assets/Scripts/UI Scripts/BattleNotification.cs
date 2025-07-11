using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleNotification : MonoBehaviour
{
    [SerializeField] private float _lifespan = 2f;
    private bool _isAlive = false;
    private float _currentTime;
    [SerializeField] private Text _textObject;
    private Animator _animator;
    private UiNotifier _notifier;



    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_isAlive)
            CountLifetime();
    }

    private void CountLifetime()
    {
        _currentTime += Time.deltaTime;

        if (_currentTime >= _lifespan)
        {
            //reset the anim and lifetime
            _isAlive = false;
            _currentTime = 0;
            _animator.SetBool("IsStarted", false);

            //return to inactivity
            transform.parent = _notifier.transform;
            transform.localPosition = Vector3.zero;
        }
    }

    public void SetNotifier(UiNotifier notifier) {  _notifier = notifier; }

    public void ShowNotification(string message, Transform parent, Vector3 offset)
    {
        if (!_isAlive)
        {
            transform.parent = parent;
            transform.localPosition = offset;
            _textObject.text = message;

            _animator.SetBool("IsStarted", true);
            _isAlive = true;
        }
    }
}
