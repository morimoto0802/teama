using UnityEngine;
using System.Collections;

public class HanabiEffectManager : MonoBehaviour
{

    [SerializeField] GameManager GameManager;
    [SerializeField] GameObject Quadaka;
    [SerializeField] GameObject Quadki;
    [SerializeField] GameObject Quadao;
    [SerializeField] GameObject Quadmidori;
    [SerializeField] GameObject Quadmurasaki;

    void Update()
    {
        if (Input.GetKey(KeyCode.B))
        {
            Quadki.gameObject.SetActive(true);
            Invoke("TESTki", 2.0f);
        }

        if (Input.GetKey(KeyCode.H))
        {
            Quadmidori.gameObject.SetActive(true);
            Invoke("TESTmidori", 2.0f);
        }

        if (Input.GetKey(KeyCode.J))
        {
            Quadao.gameObject.SetActive(true);
            Invoke("TESTao", 2.0f);
        }

        if (Input.GetKey(KeyCode.K))
        {
            Quadmurasaki.gameObject.SetActive(true);
            Invoke("TESTmurasaki", 2.0f);
        }

        if (Input.GetKey(KeyCode.L))
        {
            Quadaka.gameObject.SetActive(true);
            Invoke("TESTaka", 2.0f);
        }
    }
    private void TESTki()
    {
        Quadki.gameObject.SetActive(false);
    }

    private void TESTmidori()
    {
        Quadmidori.gameObject.SetActive(false);
    }

    private void TESTao()
    {
        Quadao.gameObject.SetActive(false);
    }

    private void TESTmurasaki()
    {
        Quadmurasaki.gameObject.SetActive(false);
    }

    private void TESTaka()
    {
        Quadaka.gameObject.SetActive(false);
    }
}

  