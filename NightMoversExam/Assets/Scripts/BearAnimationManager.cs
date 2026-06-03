using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearAnimationManager : MonoBehaviour
{
    [SerializeField]
    private List<string> animationBools;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ActivateBear()
    {
        StartCoroutine(PlayChaseAnimations());
    }

    public void TriggerExplosionAnim()
    {
        StopAllCoroutines();
        if (animationBools.Count > 2)
            anim.SetBool(animationBools[2], true);
    }

    IEnumerator PlayChaseAnimations()
    {
        yield return new WaitForSeconds(2);
        anim.SetBool(animationBools[0], true);
        yield return new WaitForSeconds(3.15f);
        anim.SetBool(animationBools[1], true);
        yield return new WaitForSeconds(2.06f);
        anim.SetBool(animationBools[2], true);
    }
}