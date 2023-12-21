using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class exitApp : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject quitpanel;
        //public AudioSource BtnExt;

        public void BtnExtClk()
        {
        // BtnExt.Play();
        //SceneManager.LoadScene("Main");
        Application.Quit();
        }

        public void OpenAreYouSure()
        {
        Debug.Log("OpenAreYouSure");
         quitpanel.SetActive(true);
        }

        public void CloseAreYouSure()
        {
        quitpanel.SetActive(false);
        }
  }

