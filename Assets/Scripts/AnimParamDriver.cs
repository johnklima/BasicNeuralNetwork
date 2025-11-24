using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Small class to lerp a single animation parameter by name
/// </summary>
public class AnimParamDriver : MonoBehaviour
{
    public Animator animTree;
    public float T = 0;
    public bool lerpValue = false;
    public float endValue;
    public float startValue;
    public string parmName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //do it until...
        if(lerpValue)
        {
            //slerp them
            float newVal = Mathf.Lerp(startValue, endValue, T);

            //elegant!
            animTree.SetFloat(parmName, newVal);

            T += Time.deltaTime;
            if (T >= 1.1f)
            {
                //... T is complete
                T = 0;
                lerpValue = false;
            }

        }

    }
}
