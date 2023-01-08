using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class tokuten : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    int score;

    // Start is called before the first frame update
    void Start()
    {
        score = GameManager.getscore();//PlayerControllerÇ…ÉVÅ[ÉìÇÃñºëO

        ScoreText.text = string.Format("Score:{0}", score);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}