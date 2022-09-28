using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SoundEffectManager : MonoBehaviour
{

    [SerializeField] GameManager GameManager;
    [SerializeField] AudioSource akaPlayer;
    [SerializeField] AudioSource kiPlayer;
    [SerializeField] AudioSource aoPlayer;
    [SerializeField] AudioSource midoriPlayer;
    [SerializeField] AudioSource murasakiPlayer;

    void OnEnable()
    {
        GameManager
          .OnSoundEffect
          .Where(type => type == "aka")
          .Subscribe(type => akaPlay());

        GameManager
          .OnSoundEffect
          .Where(type => type == "ki")
          .Subscribe(type => kiPlay());

        GameManager
          .OnSoundEffect
          .Where(type => type == "ao")
          .Subscribe(type => aoPlay());

        GameManager
          .OnSoundEffect
          .Where(type => type == "murasaki")
          .Subscribe(type => murasakiPlay());

        GameManager
          .OnSoundEffect
          .Where(type => type == "midori")
          .Subscribe(type => midoriPlay());
    }

    void akaPlay()
    {
        akaPlayer.Stop();
        akaPlayer.Play();
    }

    void kiPlay()
    {
        kiPlayer.Stop();
        kiPlayer.Play();
    }

    void aoPlay()
    {
        aoPlayer.Stop();
        aoPlayer.Play();
    }

    void murasakiPlay()
    {
        murasakiPlayer.Stop();
        murasakiPlayer.Play();
    }

    void midoriPlay()
    {
        midoriPlayer.Stop();
        midoriPlayer.Play();
    }
}