using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class MainText : MonoBehaviour
{
    [TextArea(3, 10)]
    public string mobileText;

    void Start()
    {
        if (IsWebGLMobile.Get() && mobileText != null)
        {
            GetComponent<Text>().text = mobileText;
        }
    }
}
