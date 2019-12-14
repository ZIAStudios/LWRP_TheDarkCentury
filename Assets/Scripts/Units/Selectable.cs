using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    
    Renderer r;
    [SerializeField] bool unselectedObject = false;
    GameObject visible;

    private void Awake()
    {
        r = GetComponentInChildren<Renderer>();
        

    }

    private void Start()
    {
        //if (!unselectedObject)
            visible = gameObject.transform.GetChild(0).gameObject;
        //r.material.color = norColor;
        //if (!unselectedObject)
            visible.SetActive(false);
    }

    #region When item is selected
    internal bool isSelected
    {
        get
        {
            return _isSelected;
        }
        set
        {
            if (!unselectedObject)
            {
                _isSelected = value;
                //Replace this with your custom code. What do you want to happen to a Selectable when it get's (de)selected?

                if (value) visible.SetActive(true);
                else visible.SetActive(false);

                /*if (r != null)
                {
                    r.material.color = value ? selColor : norColor;
                }*/
            }

        }
    }

    private bool _isSelected;

    void OnEnable()
    {
        Units_Selection.selectables.Add(this);
    }

    void OnDisable()
    {
        Units_Selection.selectables.Remove(this);
    }
    #endregion


}
