using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateMenuOnClick : MonoBehaviour
{
    [SerializeField] private List<Button> _categoryButtons= new();
    private Button _lastCategorySelection;






    public void UpdateMenu(Button latestClick)
    {
        //only update if one of the menu buttons were clicked
        if (_categoryButtons.Contains(latestClick))
        {
            //if a different button was clicked
            if (latestClick != _lastCategorySelection)
            {
                if (_lastCategorySelection == null)
                {
                    //update the last selection
                    _lastCategorySelection = latestClick;
                    return;
                }
                    
                //get the clicked button's toggle controller
                ToggleUiOnClick lastSelectionsToggleController = _lastCategorySelection.GetComponent<ToggleUiOnClick>();

                //close the last selection's menu if it's open
                if (lastSelectionsToggleController.IsTargetActive())
                    lastSelectionsToggleController.ToggleTargetUi();

                //update the last selection
                _lastCategorySelection = latestClick;
            }   
        }
    }
}
