using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODManager : Singleton<FMODManager>
{
    private FMOD.Studio.EventInstance _bgm;
    private FMOD.Studio.EventInstance _sword;
    private FMOD.Studio.EventInstance _hit;
    private FMOD.Studio.EventInstance _death;

    void Start()
    {
        _bgm = FMODUnity.RuntimeManager.CreateInstance("event:/BGM");
        _sword = FMODUnity.RuntimeManager.CreateInstance("event:/Sword");
        _hit = FMODUnity.RuntimeManager.CreateInstance("event:/Hit");
        _death = FMODUnity.RuntimeManager.CreateInstance("event:/Death");
        StartBGM();
    }

    void Update()
    {
        
    }

    public void StartBGM()
    {
        _bgm.start();
    }

    public void StopBGM()
    {
        _bgm.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void PlaySwordSound()
    {
        _sword.start();
    }

    public void PlayHitSound(int intensity)
    {
        _hit.setParameterByName("Intensity", intensity);
        _hit.start();
    }

    public void PlayDeathSound()
    {
        _death.start();
    }

}
