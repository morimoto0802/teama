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

    void OnEnable()
    {
        if (Input.GetKey(KeyCode.B))
        {
            Quadaka.gameObject.SetActive(true);
        }

        if (Input.GetKey(KeyCode.H))
        {
            Quadki.gameObject.SetActive(true);
        }

        if (Input.GetKey(KeyCode.J))
        {
            Quadao.gameObject.SetActive(true);
        }

        if (Input.GetKey(KeyCode.K))
        {
            Quadmidori.gameObject.SetActive(true);
        }

        if (Input.GetKey(KeyCode.L))
        {
            Quadmurasaki.gameObject.SetActive(true);
        }
    }
}