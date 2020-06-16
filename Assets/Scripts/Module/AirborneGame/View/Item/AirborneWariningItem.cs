using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneWariningItem : MonoBehaviour {

    public float LifeMaxTime = 2.0f;
     float life = 0;
    private void OnEnable()
    {
        Debug.LogError("AirborneWariningItem OnEnable");
        life = 0;
    }
    private void FixedUpdate()
    {
        life += Time.deltaTime;
        if (life < LifeMaxTime)
            return;
        transform.gameObject.SetActive(false);
    }
}
