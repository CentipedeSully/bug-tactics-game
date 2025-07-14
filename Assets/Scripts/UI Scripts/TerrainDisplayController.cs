using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainDisplayController : MonoBehaviour
{
    [SerializeField] private GameObject _displayObject;
    [SerializeField] private Text _terrainName;
    [SerializeField] private GameObject _foodYield;
    [SerializeField] private Text _foodNumber;
    [SerializeField] private GameObject _workYield;
    [SerializeField] private Text _workNumber;
    [SerializeField] private Text _terrainDesc;







    public void SetName(string name) {  _terrainName.text = name; }
    public void SetDesc(string description) {  _terrainDesc.text = description; }
    public void SetFoodNumber(int number)
    {
        _foodYield.SetActive(true);

        //show the yield if it yields or COSTS food
        if (number < 0)
            _foodNumber.text = $"{number}";
        else if (number > 0)
            _foodNumber.text = $"+{number}";

        //hide the yield if it's zero
        else
            _foodYield.SetActive(false);

        
    }
    public void SetWorkNumber(int number)
    {
        _workYield.SetActive(true);

        //show the yield if it yields or COSTS food
        if (number < 0)
            _workNumber.text = $"{number}";
        else if (number > 0)
            _workNumber.text = $"+{number}";

        //hide the yield if it's zero
        else
            _workYield.SetActive(false);

        
    }

    public void ShowTerrainDisplay(string name, string desc, int foodYield, int workYield)
    {
        _displayObject.SetActive(true);
        SetName(name);
        SetDesc(desc);
        SetFoodNumber(foodYield);
        SetWorkNumber(workYield);
    }

    public void HideTerrainDisplay() {  _displayObject.SetActive(false); }
}
