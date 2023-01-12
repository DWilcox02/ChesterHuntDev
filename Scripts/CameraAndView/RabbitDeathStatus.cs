using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RabbitDeathStatus : MonoBehaviour
{
    private int kills;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        kills = 0;
    }

    public void AddKill()
    {
        kills = kills + 1;
        text.text = kills.ToString();
    }
}
