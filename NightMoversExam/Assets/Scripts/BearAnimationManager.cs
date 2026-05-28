using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class BearAnimationManager : MonoBehaviour
{
    [SerializeField]
    private List<string> animationBools;
    private Animator anim;


    private void Start()
    {
        ActivateBear();
        anim = GetComponent<Animator>();
    }
    public void ActivateBear()
    {
        StartCoroutine(PlayAnimations());
    }

    IEnumerator PlayAnimations()
    {
        yield return new WaitForSeconds(2);
        anim.SetBool(animationBools[0], true);
        yield return new WaitForSeconds(3.15f);
        anim.SetBool(animationBools[1], true);
        yield return new WaitForSeconds(2.06f);
        anim.SetBool(animationBools[2], true);

    }
}
