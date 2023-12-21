using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InputHelper : MonoBehaviour
{
    public InputField clearIt;

    public void submittingClear()
    {
        clearIt.text = "";
    }
}
