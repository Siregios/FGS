using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TEst : MonoBehaviour {

    Image b;
    public Health a;

    private void Start()
    {
        b = GetComponent<Image>();
    }

    private void Update()
    {
        b.fillAmount = a.stun;
    }

}
