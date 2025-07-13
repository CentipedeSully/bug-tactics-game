using SullysToolkit.TableTop;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IndicatorManager : MonoBehaviour
{
    [SerializeField] private GameBoard _board;
    [SerializeField] private GameObject _clickIndicatorPrefab;
    [SerializeField] private GameObject _cellSelectionIndicatorPrefab;

    [SerializeField] private Transform _activeClickIndicatorContainer;
    [SerializeField] private Transform _inactiveClickIndicatorContainer;

    [SerializeField] private Transform _activeCellSelectionIndicatorContainer;
    [SerializeField] private Transform _inactiveCellSelectionIndicatorContainer;
    
    Dictionary<(int,int),GameObject> _activeCellSelectionIndicators = new Dictionary<(int,int),GameObject>();






    private void CreateClickIndicator()
    {
        //create a new click indicator and parent it into the respective inactivity container
        GameObject newClickIndicatorObject = Instantiate(_clickIndicatorPrefab, _inactiveClickIndicatorContainer);
        ClickIndicator clickScript = newClickIndicatorObject.GetComponent<ClickIndicator>();

        //init the clickIndicator's home while Inactive (it'll return to this when it's lifetime expires)
        clickScript.SetInactiveHome(_inactiveClickIndicatorContainer);
        newClickIndicatorObject.SetActive(false);

    }

    private void CreateCellSelectionIndicator()
    {
        //create a new selectedCell indicator and parent it into the respective inactivity container
        GameObject newSelectionIndicator = Instantiate(_cellSelectionIndicatorPrefab, _inactiveCellSelectionIndicatorContainer);

        //hide the new cell indicator
        newSelectionIndicator.SetActive(false);
    }




    public void PlaceClickIndicator((int,int) cell)
    {
        //create more if we don't have any atm
        if (_inactiveClickIndicatorContainer.transform.childCount == 0)
            CreateClickIndicator();

        //raise an error if we've somehow failed to create a new indicator
        if (_inactiveClickIndicatorContainer.transform.childCount == 0)
        {
            Debug.LogWarning("Failed to create a click indicator for some reason");
            return;
        }

        //move an inactive click indicator to the active container
        Transform selectedClickIndicator = _inactiveClickIndicatorContainer.GetChild(0);
        selectedClickIndicator.parent = _activeClickIndicatorContainer;

        //move the inactive indicator to it's requested position
        selectedClickIndicator.position = _board.GetGrid().GetPositionFromCell(cell.Item1,cell.Item2);

        //activate the indicator
        selectedClickIndicator.gameObject.SetActive(true);
        selectedClickIndicator.GetComponent<ClickIndicator>().Activate();

        //The indicator will return to it's inactivity home once it expires

            
    }

    public void PlaceSelectionIndicator((int, int) cell)
    {
        if (_activeCellSelectionIndicators.ContainsKey(cell))
        {
            Debug.Log($"Cell {cell} is already occupied by a cell Indicator. Ignoring request");
            return;
        }

        //create more if we don't have any atm
        if (_inactiveCellSelectionIndicatorContainer.transform.childCount == 0)
            CreateCellSelectionIndicator();

        //raise an error if we've somehow failed to create a new indicator
        if (_inactiveCellSelectionIndicatorContainer.transform.childCount == 0)
        {
            Debug.LogWarning("Failed to create a Cell Selection indicator for some reason");
            return;
        }

        //move an inactive cell selection indicator to the active container
        Transform chosenCellIndicator = _inactiveCellSelectionIndicatorContainer.GetChild(0);
        chosenCellIndicator.parent = _activeCellSelectionIndicatorContainer;

        //move the inactive indicator to it's requested position
        chosenCellIndicator.position = _board.GetGrid().GetPositionFromCell(cell.Item1, cell.Item2);

        //add this new indicator to our collection of active indicators
        _activeCellSelectionIndicators.Add(cell, chosenCellIndicator.gameObject);

        //activate the indicator
        chosenCellIndicator.gameObject.SetActive(true);

        //These need to be cleared manually
    }

    public void RemoveCellIndicator((int, int) cell)
    {
        if (_activeCellSelectionIndicators.ContainsKey(cell))
        {
            //collect, then remove the specified indicator from the target position
            GameObject indicator = _activeCellSelectionIndicators[cell];
            _activeCellSelectionIndicators.Remove(cell);

            //return the indicator to the inactive container, then deactivate it
            indicator.transform.parent = _inactiveCellSelectionIndicatorContainer;
            indicator.SetActive(false);

            //now it's ready for reuse
        }
    }

    public void RemoveAllSelectionIndicators()
    {
        _activeCellSelectionIndicators.Clear();

        //remove each child from the active container, and add them to the inactive container
        for (int i= _activeCellSelectionIndicatorContainer.childCount -1; i >= 0; i--)
        {
            //work from the last towards the front
            Transform indicatorTransform = _activeCellSelectionIndicatorContainer.GetChild(i);
            indicatorTransform.parent = _inactiveCellSelectionIndicatorContainer;

            //don't forget to deactivate each one
            indicatorTransform.gameObject.SetActive(false);
        }
    }
    
}
