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
        if (animationBools.Count > 0)
        {
            yield return new WaitForSeconds(2f);
            anim.SetBool(animationBools[0], true);
        }

        if (animationBools.Count > 1)
        {
            yield return new WaitForSeconds(3.15f);
            anim.SetBool(animationBools[1], true);
        }
    }
}