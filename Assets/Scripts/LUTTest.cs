
using System;
using UnityEngine;
using UnityEngine.Serialization;

//this LUT will be an animation LUT based on player to player type
// note that cols and rows in this grid are off. Sensei should be after warrior to mirror the prefab
// code indexes however ARE correct

// ME           ||                                 THEM
// _____________||__________________________________________________________________________________________
// PLAYER TYPE  || Geisha   |   Ninja   | Sam Grnt | Sam War   |   Sensei  |  Village Man  |  Village Woman
// _____________||__________|___________|__________|___________|___________|_______________|________________
// Geisha       ||  chat    |    flirt  |  ignore  |   flirt   |   bow     |    insult     |    insult
// _____________||__________|___________|__________|___________|___________|_______________|________________
// Ninja        || downlook |   chat    |  insult  |   chat    |   chat    |    whisper    |     bless
// _____________||__________|___________|__________|___________|___________|_______________|________________
// Sam Grnt     ||  flirt   |   agro    |  whisper |   insult  |  ignore   |    whisper    |    whisper
// _____________||__________|___________|__________|___________|___________|_______________|________________
// Sam War      ||  flirt   |   chat    |  whisper |   agro    |   chat    |    insult     |    insult
// _____________||__________|___________|__________|___________|___________|_______________|________________
// Sensei       ||  flirt   |   chat    |  ignore  |   chat    |   chat    |    insult     |    insult
// _____________||__________|___________|__________|___________|___________|_______________|________________
// Vil Man      ||  bow     |   bow     |  whisper |   bow     |   bow     |    chat       |    flirt
// _____________||__________|___________|__________|___________|___________|_______________|________________
// Vil Woman    ||  bow     |   bow     |  whisper |   bow     |   bow     |    flirt      |     chat
// _____________||__________|___________|__________|___________|___________|_______________|________________


public class LUTTest : MonoBehaviour
{

    /// <summary>
    /// The Animation Params
    /// </summary>
    public int CHAT = 0;
    public int BOW = 1;
    public int FLIRT = 2;
    public int INSULT = 3;
    public int BLESS = 4;
    public int WHISPER = 5;
    public int AGRO = 6;
    public int SNOB = 7;
    public int IGNORE = 8;

    public GameObject animNNTemplate;
    /// <summary>
    /// The Character Types. Order matters!!! as they appear in the prefab
    /// </summary>
    public int GEISHA = 0;
    public int NINJA = 1;
    public int GRUNT = 2;
    public int WARRIOR = 3;
    public int SENSEI = 4;
    public int MAN = 5;
    public int WOMAN = 6;

    public string[] typenames = { "GEISHA", "NINJA", "GRUNT", "WARRIOR", "SENSEI", "MAN", "WOMAN" };
    
    //identifiers
    public int whatAmI = 0;   //my type, auto assigned
    public string whoAmI;     //my actual name
    public int myID;

    //dimensions of the lut
    public int cols, rows;
    
    public int[,] resultTable;     //where we stick the animation numbers (names of parms by index)
    public string[] animParms;     //names of the animation parms
    private Animator AnimTree;     //the tree that cares about the anim parms
    
    /// <summary>
    /// If nobody knows anybody, the default knee jerk reaction to the character type applies
    /// So what we actually do here, is retrain an individual when encountering another individual
    /// It starts with the knee jerk reaction, but then can evolve into a person-to-person relationship
    /// </summary>
    private void Start()
    {
        Debug.Log("-------------BUILD LUT----------------");
        
        //get my start char type
        for (int i = 0; i < typenames.Length; i++)
        {
            //array of typenames must mirror the prefab children
            if (transform.GetChild(i).gameObject.activeInHierarchy)
            {
                //this is who I am based on the enabled child in the prefab
                whatAmI = i;
                whoAmI = transform.GetChild(i).name;
                Debug.Log("I am a " + typenames[i] + " and my name is " + whoAmI);
            }
        }
        AnimTree = GetComponent<Animator>();
        
        //rubber meets road eventually
        rows = 7;
        cols = 7;
        //scope this, maybe read a file rather
        
        //hmmmmmm
        myID = whoAmI.GetHashCode();
        
        //get all the names of the parms
        animParms = new string[AnimTree.parameterCount];
        for (int i = 0; i < AnimTree.parameterCount; i++)
        {
            animParms[i] = AnimTree.parameters[i].name;
        }
        
        resultTable = new int[rows, cols];

        //Geisha
        resultTable[GEISHA, GEISHA] = CHAT;
        resultTable[GEISHA, NINJA] = FLIRT;
        resultTable[GEISHA, GRUNT] = IGNORE;
        resultTable[GEISHA, WARRIOR] = FLIRT;
        resultTable[GEISHA, SENSEI] = BOW;
        resultTable[GEISHA, MAN] = INSULT;
        resultTable[GEISHA, WOMAN] = INSULT;


        //Ninja
        resultTable[NINJA, GEISHA] = FLIRT;
        resultTable[NINJA, NINJA] = WHISPER;
        resultTable[NINJA, GRUNT] = INSULT;
        resultTable[NINJA, WARRIOR] = IGNORE;
        resultTable[NINJA, SENSEI] = AGRO;
        resultTable[NINJA, MAN] = CHAT;
        resultTable[NINJA, WOMAN] = CHAT;

        //Grunt
        resultTable[GRUNT, GEISHA] = FLIRT;
        resultTable[GRUNT, NINJA] = WHISPER;
        resultTable[GRUNT, GRUNT] = AGRO;
        resultTable[GRUNT, WARRIOR] = CHAT;
        resultTable[GRUNT, SENSEI] = CHAT;
        resultTable[GRUNT, MAN] = INSULT;
        resultTable[GRUNT, WOMAN] = INSULT;

        //Warrior
        resultTable[WARRIOR, GEISHA] = FLIRT;
        resultTable[WARRIOR, NINJA] = IGNORE;
        resultTable[WARRIOR, GRUNT] = CHAT;
        resultTable[WARRIOR, WARRIOR] = CHAT;
        resultTable[WARRIOR, SENSEI] = CHAT;
        resultTable[WARRIOR, MAN] = INSULT;
        resultTable[WARRIOR, WOMAN] = INSULT;

        //Sensei
        resultTable[SENSEI, GEISHA] = SNOB;
        resultTable[SENSEI, NINJA] = IGNORE;
        resultTable[SENSEI, GRUNT] = CHAT;
        resultTable[SENSEI, WARRIOR] = CHAT;
        resultTable[SENSEI, SENSEI] = CHAT;
        resultTable[SENSEI, MAN] = BLESS;
        resultTable[SENSEI, WOMAN] = BLESS;


        //Vil Man
        resultTable[MAN, GEISHA] = BOW;
        resultTable[MAN, NINJA] = WHISPER;
        resultTable[MAN, GRUNT] = BOW;
        resultTable[MAN, WARRIOR] = BOW;
        resultTable[MAN, SENSEI] = BOW;
        resultTable[MAN, MAN] = CHAT;
        resultTable[MAN, WOMAN] = FLIRT;

        //Vil Woman
        resultTable[WOMAN, GEISHA] = BOW;
        resultTable[WOMAN, NINJA] = WHISPER;
        resultTable[WOMAN, GRUNT] = BOW;
        resultTable[WOMAN, WARRIOR] = BOW;
        resultTable[WOMAN, SENSEI] = BOW;
        resultTable[WOMAN, MAN] = FLIRT;
        resultTable[WOMAN, WOMAN] = CHAT;
        
        //debug log the table
        for (int otherType = 0; otherType < cols; otherType++)
        {
            int encounter = resultTable[whatAmI, otherType];
            
            Debug.Log("I am " + whoAmI +  
                      " my hashcode is " + myID +
                      " I am a " + typenames[whatAmI] + 
                      " and when I meet a " + typenames[otherType] + 
                      " by default I will do " + animParms[encounter]);
        
        }
        
        //add NN children
        for (int nn = 0; nn < AnimTree.parameterCount; nn++)
        {
            GameObject nnobj = Instantiate(animNNTemplate, transform);
            nnobj.name = whoAmI + animParms[nn];
            nnobj.transform.GetComponent<AnimParamDriver>().parmName = animParms[nn];
            nnobj.transform.GetComponent<AnimParamDriver>().animTree = AnimTree;
            nnobj.transform.GetComponent<NN_base>().filename = nnobj.name;
            nnobj.transform.GetComponent<NN_Trainer>().paramName = animParms[nn];
            
        }
    }

    public int them = 0;
    private void Update()
    {
       
       
        
    }

    public void Train( int themtype)
    {
        
        
        
    }
 
}
