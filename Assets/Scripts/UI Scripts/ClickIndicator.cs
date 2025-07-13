using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickIndicator : MonoBehaviour
{
    private Transform _inactiveHome;
    [SerializeField] private float _lifespan;
    private float _currentTime = 0;
    private bool _isAlive = false;





    //monobehaviours
    private void Update()
    {
        if (_isAlive)
            CountLifespan();
    }





    //internals
    private void CountLifespan()
    {
        _currentTime += Time.deltaTime;


        if (_currentTime >= _lifespan)
        {
            //reset the fields and turn off the object
            _isAlive = false;
            _currentTime = 0;
            transform.parent = _inactiveHome;
            transform.localPosition = Vector3.zero;
            gameObject.SetActive(false);
        }
    }




    //Externals
    public void SetInactiveHome(Transform newParent)
    {
        if (newParent == null)
        {
            Debug.LogError($"Attempted to set the inactive home to NULL. " +
                $"This needs to be set for the object recycling to work properly");
            return;
        }

        _inactiveHome = newParent;
    }

    public void Activate()
    {
        if (!_isAlive && _inactiveHome != null)
            _isAlive=true;
        else if (_inactiveHome == null)
            Debug.LogError("Attempted to activate a clickIndicator without setting it's inactive home. Do this before activating clickIndicators");
    }







}
