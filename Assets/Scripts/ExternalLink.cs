using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ExternalLink : MonoBehaviour
{
    public string href;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(onClick);
    }

    void onClick()
    {
        Application.OpenURL(href);
    }
}
