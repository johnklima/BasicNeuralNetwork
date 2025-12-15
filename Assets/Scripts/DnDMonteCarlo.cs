using UnityEngine;

public class DnDMonteCarlo : MonoBehaviour
{
    public int d4s = 0;
    public int d6s = 0;
    public int d8s = 0;
    public int d10s = 0;
    public int d20s = 0;
    public int maxRoll = 0;

    public bool runit = false;
    public bool add = false;
    public bool onesAndTens = false;
    public bool multiple = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform template = transform.GetChild(0);
        
        for(int i = 1; i < 100; i++)
        {
            GameObject obj = Instantiate(template.gameObject, template.position+Vector3.right * i, 
                                         template.rotation, template.parent);
            
            obj.name = "Bucket (" + i + ")";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (runit)
        {
            int [] rolls4 = new int [d4s];
            int [] rolls6 = new int [d6s];
            int [] rolls8 = new int [d8s];
            int [] rolls10 = new int [d10s];
            int [] rolls20 = new int [d20s];

            for (int r = 0; r < d4s; r++)
            {
                rolls4[r] = Random.Range(1, 5);
            }
        
            for (int r = 0; r < d6s; r++)
            {
                rolls6[r] = Random.Range(1, 7);
            }
        
            for (int r = 0; r < d8s; r++)
            {
                rolls8[r] = Random.Range(1, 9);
            }
        
            for (int r = 0; r < d10s; r++)
            {
                rolls10[r] = Random.Range(0, 10);
            }
        
            for (int r = 0; r < d20s; r++)
            {
                rolls20[r] = Random.Range(1, 21);
            }

            if (onesAndTens)
            {
                maxRoll = 100;
                doOnesAndTens(rolls10);
            }
            if (multiple)
            {
                maxRoll = 100;
            }  
            
        }
        
    }

    void doOnesAndTens(int[] _rolls10)
    {
        int total = _rolls10[0] * 10 + _rolls10[1] ;
        if (total == 0)
        {
            total = 100;
        }
        //Debug.Log(total);
        //lets not forget the array is zero based
        //add a "bean" to the bucket
        transform.GetChild(total-1).localScale += Vector3.up * 0.1f;
        transform.GetChild(total-1).position = Vector3.zero + Vector3.right * (total-1) + Vector3.up * 0.1f;
    }

    void doMultiple()
    {
        
    }
    void doAddition()
    {
    }
    void doMultiplication()
    {
        
    }
}
