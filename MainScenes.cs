using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
   
public class MainScenes : MonoBehaviour
{   
    public float delaytime;
    public string nameScene;
    
    public void StartGame(){
        StartCoroutine(LoadAfterDelay());
    }
    IEnumerator LoadAfterDelay(){
        yield return new WaitForSeconds(delaytime);
        SceneManager.LoadScene(nameScene);
    }
    public void ThoatGame(){
        Application.Quit();
    }
}
