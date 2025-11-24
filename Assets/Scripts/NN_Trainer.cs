using UnityEngine;
using UnityEngine.Serialization;

public class NN_Trainer : MonoBehaviour
{

    public NN_base NN;
    public LUTTest LUT;
    public Transform NPCs;
    
    public bool defaulttrain;

    public string paramName;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (defaulttrain)
        {
            NN = transform.GetComponent<NN_base>();
            LUT = transform.parent.GetComponent<LUTTest>();
            NPCs = transform.parent.parent.GetComponent<Transform>();    
        
            Debug.Log(LUT.typenames[LUT.whatAmI] + " has a trainer ");
            //y here, is the actual person, one record per person
            //TODO: expand the training set as more people are added - not easy
            
            int y = 0 ;
            float value = 0;
            foreach (Transform npc in NPCs)
            {
                if (npc != transform.parent)
                {
                    LUTTest npcLUT = npc.GetComponent<LUTTest>(); 
                    int encounter = LUT.resultTable[LUT.whatAmI, npcLUT.whatAmI];

                    //I am me, and this is the specific animation, so all
                    //that matters is that this entry is a zero, or a one

                    if (paramName == LUT.animParms[encounter])
                    {
                        value = 1.0f;
                        Debug.Log(npc.name + " the other " +  paramName + " " + transform.name + " is the default" );
                    }
                    else
                    {
                        value = 0.0f;
                    }
                    
                    NN.trainX[y][0] = npcLUT.whatAmI;   //or what they are
                    NN.trainX[y][1] = npcLUT.myID;      //or who they are
                    NN.trainY[y] = value;                //the result
                }

                y++;
            }

            NN.retrain = true;
            defaulttrain = false;
            
        }
    }
}
