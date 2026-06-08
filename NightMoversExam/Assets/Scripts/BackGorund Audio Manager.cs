using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class BackGorundAudioManager : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> RandomAudioClips;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayRandomSounds());
    }

    IEnumerator PlayRandomSounds()
    {
        yield return new WaitForSeconds(Random.Range(10,18));
        audioSource.clip = RandomAudioClips[Random.Range(0, RandomAudioClips.Count)];
        audioSource.Play();
        StartCoroutine(PlayRandomSounds());

    }
}
