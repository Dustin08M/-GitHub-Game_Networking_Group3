using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGreetMenu : MonoBehaviour
{
    public Text greetingText;
    void Start()
    {
        greetingText.text = "Hello, " + AuthenticationManager2.PlayerName.ToString();
    }
}
