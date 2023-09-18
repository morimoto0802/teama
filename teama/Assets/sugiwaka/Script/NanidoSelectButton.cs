using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NanidoSelectButton: MonoBehaviour
{
    public string sceneName; // シーン名を格納する変数

    public void Onclick()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();

        Invoke("Select", 0.8f);
    }
    public void Select()
    {
        //ここで移りたいシーンを指定します。
        SceneManager.LoadScene("sceneName");
    }
}

