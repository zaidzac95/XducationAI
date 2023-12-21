using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainScene : MonoBehaviour
    
{
   
    //public int a = 1;
    //public float Delay = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LoadintroScene()
    {
        SceneManager.LoadScene(0);
    }

    public void ReloadCurrentScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

}

