using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanTweenShow : MonoBehaviour
{
    public RectTransform panelRT;
    // Start is called before the first frame update
    void Start()
    {
        
    }

   public void onShowclick()
    {
        
        LeanTween.moveY(panelRT, 0, 0.5f);
    }

    public void onHideClick()
    {
        LeanTween.moveY(panelRT, -400, 0.5f);
    }
}
