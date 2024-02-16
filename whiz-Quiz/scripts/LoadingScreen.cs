using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen instance;
    public Image firstImage;
    public Image secondImage;

    public void Start()
    {
        firstImage.gameObject.SetActive(true);
        secondImage.gameObject.SetActive(false);
        StartCoroutine(loadingScreen());
    }
    IEnumerator loadingScreen()
    {
        yield return new WaitForSeconds(3f);
        firstImage.gameObject.SetActive(false);
        secondImage.gameObject.SetActive(true);
    }
    public void onButton()
    {
        SceneManager.LoadScene(1);
    }
}
