using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NanidoSelectButton: MonoBehaviour
{
    public string sceneName; // �V�[�������i�[����ϐ�

    public void Onclick()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();

        Invoke("Select", 0.8f);
    }
    public void Select()
    {
        //�����ňڂ肽���V�[�����w�肵�܂��B
        SceneManager.LoadScene("sceneName");
    }
}

