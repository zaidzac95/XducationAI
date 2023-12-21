using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource BtnEnt;
    
    public void BtnEntClk()
    {
        BtnEnt.Play();
        SceneManager.LoadScene("Owl_s_Post_Office_demo");
    }
}
