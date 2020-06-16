using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicRhythmClickAreasComment : MonoBehaviour
{
    Animator _animator;
    private void Awake()
    {
        _animator = transform.GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayShortOnce(Tick tick)
    {
        // Debug.LogError("PlayShortOnce ................");
        _animator.SetTrigger("ShortDown");
    }


    public void PlayLongDown(Tick tick)
    {
        Debug.LogError("PlayShortOnce .............LongDown...");
        _animator.SetTrigger("LongDown");
    }
    public void PlayLongUp(Tick tick)
    {
        Debug.LogError("PlayShortOnce ........PlayLongUp........");
        _animator.SetTrigger("LongUp");
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.T))
    //    {
    //        Tick tick = new Tick();
    //        tick.Way = 1;
    //        PlayShortOnce(tick);
    //    }

    //    if (Input.GetKeyDown(KeyCode.L))
    //    {
    //        Tick tick = new Tick();
    //        tick.Way = 1;
    //        PlayLongDown(tick);
    //    }
    //    if (Input.GetKeyUp(KeyCode.L))
    //    {
    //        Tick tick = new Tick();
    //        tick.Way = 1;
    //        PlayLongUp(tick);
    //    }
    //}
}
