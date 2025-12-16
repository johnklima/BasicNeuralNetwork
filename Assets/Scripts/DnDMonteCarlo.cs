using Unity.VisualScripting;
using UnityEditor.Rendering.CustomRenderTexture.ShaderGraph;
using UnityEngine;

public class DnDMonteCarlo : MonoBehaviour
{
    public int d4s = 0;
    public int d6s = 0;
    public int d8s = 0;
    public int d10s = 0;
    public int d20s = 0;
    public int maxRoll = 0;
    public int numRolls = 0;
    public bool runit = false;
    public bool add = false;
    public bool onesAndTens = false;
    public bool multiple = false;

    public bool doJosh = false;
    public bool doJosh2 = false;

    public Material[] mats = new Material [6];
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        /*
            can you do a curve for draw steel with the 2d10s - snake eyes are crit fails, 19 - 20 is a crit, 
            though the ranges of success I want to see on a curve (coloured?) 
            Tier 1: 11 or lower. 
            Tier 2: 12-16
            Tier 3: 17 or higher
        */

        
        Transform template = transform.GetChild(0);
        
        for(int i = 1; i < 100; i++)
        {
            GameObject obj = Instantiate(template.gameObject, template.position+Vector3.right * i, 
                                         template.rotation, template.parent);
            
            obj.name = "Bucket (" + i + ")";

            float cf = (float)i / 100.0f ;

            cf = 20.0f * cf;

            Debug.Log(cf + 1);
           
            
            int c = (int) (cf + 1);
            if (c > 18)
                obj.GetComponent<Renderer>().material = mats[3];
            else if ( c > 17 )
                obj.GetComponent<Renderer>().material = mats[3];
            else if ( c > 12)
                obj.GetComponent<Renderer>().material = mats[2];
            else if ( c > 2)
                obj.GetComponent<Renderer>().material = mats[1];
            else if ( c > 0)
                obj.GetComponent<Renderer>().material = mats[0];

        }
    }

    // Update is called once per frame
    void Update()
    {

        if (runit)
        {


            numRolls++;


            if (doJosh)
            {
                DoJosh();
                return;
            }

            if(doJosh2)
            {
                DoJosh2();
                return;
            }

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
            if(add)
            {
                maxRoll = 12;
                doAddition(rolls6);
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
        //add a "bean" to the buck  et
        GraphMe(total);
      }

    void GraphMe(int total)
    {
        transform.GetChild(total-1).localScale += Vector3.up * 0.1f;
        transform.GetChild(total-1).position = Vector3.zero 
                                               + Vector3.right * (total-1) 
                                               + Vector3.up * (transform.GetChild(total-1).localScale.y / 2);

    }
    void doMultiple()
    {
        
    }
    void doAddition(int[] _rolls6)
    {
        float total = ((_rolls6[0] + _rolls6[1] ) / 12.0f ) * 100.0f ;
        Debug.Log(total);
        int itotal = ((int)total);
        GraphMe(itotal);
    }
    void doMultiplication()
    {
        
    }
    void DoJosh()
    {
        /*
            ok, so here's a real world for you
            we currently run with d20s for skill checks. Pathfinder and dnd add flat modifiers for skills. 
            What does a d4 add, or 2d4s in terms of success?
            I guess this is D&D bless and inspiration/guidance - but it'll be interesting to see
            Prof John — 12:15 PM
            So 1d20 for base, plus 1d4 or 2d 4 mods, to achieve minimum success, with some other table value to be a crit?
            Joshua
            Joshua — 12:18 PM
            I could make it messy for fun. 

            A crit would be a 20 on the d20 or a 4 on both of the 4s as long as the result is a success (lets say a DC 17).
            */

        int minimumRole = 17;
        int roll = Random.Range(1, 21); //ints are max exclusive
        int d41 = Random.Range(1, 5);
        int d42 = Random.Range(1, 5);
        int total = roll + d41 + d42;
        bool crit = false;
        bool success = false;

        int value = 0;

        d41 = 0; //just one die
        d42 = 0; //and no die
        //now we have to start making ifs
        if( d41 == 4 && d42 == 4  && 8 + roll >= 17)  //double 4s and total success
        {
            crit = true;
            success = true;
            value = 3;
        }
        else if (roll >= 20)  //regardless of the fours
        {
            crit = true;
            success = true;
            value = 3;
        }
        else if(roll + d41 + d42 >= 17) 
        {            
            success = true;
            value = 2;
        }
        else
        {
            value = 1;
        }

        //lets just make a 3 point graph, given the hard outcome, failure, success, and crit
        GraphMe(value);


     }
    void DoJosh2()
    {
        /*
            can you do a curve for draw steel with the 2d10s - snake eyes are crit fails, 19 - 20 is a crit, 
            though the ranges of success I want to see on a curve (coloured?) 
            Tier 1: 11 or lower. 
            Tier 2: 12-16
            Tier 3: 17 or higher
        */

        int d10one = Random.Range(1, 11);  //max exclusive
        int d10two = Random.Range(1, 11);

        bool critFail = false;
        bool critSuccess = false;
        int total = d10one + d10two;
        int natural = total;
        
        if (total <= 2)
        {
            critFail = true;
        }
        if(total >= 19 )
        {
            critSuccess = true;
        }
        if (!critFail && !critSuccess )
        {
            //only when there is no base crit, do we add the mod 
            int mod = Random.Range(1, 5);  //the plus four mod, another die right? 2 d10 plus 1d4, max 24, but eval max 20 
            //add just a +1 buff
            mod = 1;
            total += mod;

            int mod2 = Random.Range(1, 5);
            //add just one more buff
            mod2 = 0;
            total += mod2;

            //but now I need to change colors, or handle crits as a separate event

        }

        float bucket = (float)total / 20.0f;

        if (bucket > 1.0f)
            bucket = 1.0f;

        Debug.Log((int)(bucket * 100f));
       
        total = ((int)(bucket * 100f));

        GraphMe(total);
    }

}
