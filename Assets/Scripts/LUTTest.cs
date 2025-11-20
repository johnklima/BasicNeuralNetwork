using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


//this LUT will be an animation LUT based on player to player type

// ME           ||                                 THEM
// _____________||__________________________________________________________________________________________
// PLAYER TYPE  || Geisha   |   Sensei  |  Ninja   | Sam Grnt  | Sam War   |  Village Man  |  Village Woman
// _____________||__________|___________|__________|___________|___________|_______________|________________
// Geisha       ||  chat    |    bow    |  flirt   |   ignore  |   flirt   |    insult     |    insult
// _____________||__________|___________|__________|___________|___________|_______________|________________
// Sensei       || downlook |   chat    |  insult  |   chat    |   chat    |    bless      |     bless
// _____________||__________|___________|__________|___________|___________|_______________|________________
// Ninja        ||  flirt   |   agro    |  whisper |   insult  |  ignore   |    whisper    |    whisper
// _____________||__________|___________|__________|___________|___________|_______________|________________
// Sam Grnt     ||  flirt   |   chat    |  whisper |   agro    |   chat    |    insult     |    insult
// _____________||__________|___________|__________|___________|___________|_______________|________________
// Sam War      ||  flirt   |   chat    |  ignore  |   chat    |   chat    |    insult     |    insult
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

    /// <summary>
    /// The Character Types. Order matters!!!
    /// </summary>
    public int GEISHA = 0;
    public int SENSEI = 1;
    public int NINJA = 2;
    public int GRUNT = 3;
    public int WARRIOR = 4;
    public int MAN = 5;
    public int WOMAN = 6;

    public string[] typenames = { "GEISHA", "SENSEI", "NINJA", "GRUNT", "WARRIOR", "MAN", "WOMAN" };

    public int cols, rows;
    public int[,] resultTable;
    public int[] lutData;
    public string[] animParamNames;

    private string Name;

    public Dropdown typeA;
    public Dropdown typeB;


    //start looking at them as who they are and not what they are
    public Slider inA;
    public Slider inB;


    NN_base NN;
    /// <summary>
    /// If nobody knows anybody, the default knee jerk reaction to the character type applies
    /// So what we actually do here, is retrain an individual when encountering another individual
    /// It starts with the knee jerk reaction, but then can evolve into a person-to-person relationship
    /// </summary>
    private void Start()
    {
        Debug.Log("-------------BUILD LUT----------------");
        
        Name = transform.name;
        NN = GetComponent<NN_base>();

        //rubber meets road eventually
        rows = 7;
        cols = 7;
        //if I don't have yet a file, build this default one
        if (true)
        {
            animParamNames = new string[]
            {
                "chat"      ,
                "bow"       ,
                "flirt"     ,
                "insult"    ,
                "bless"     ,
                "whisper"   ,
                "agro"      ,
                "snob"      ,
                "ignore"
            };

            resultTable = new int[rows, cols];

            //Geisha
            resultTable[GEISHA, GEISHA] = CHAT;
            resultTable[GEISHA, SENSEI] = BOW;
            resultTable[GEISHA, NINJA] = FLIRT;
            resultTable[GEISHA, GRUNT] = IGNORE;
            resultTable[GEISHA, WARRIOR] = FLIRT;
            resultTable[GEISHA, MAN] = INSULT;
            resultTable[GEISHA, WOMAN] = INSULT;


            //Ninja
            resultTable[NINJA, GEISHA] = FLIRT;
            resultTable[NINJA, SENSEI] = AGRO;
            resultTable[NINJA, NINJA] = WHISPER;
            resultTable[NINJA, GRUNT] = INSULT;
            resultTable[NINJA, WARRIOR] = IGNORE;
            resultTable[NINJA, MAN] = CHAT;
            resultTable[NINJA, WOMAN] = CHAT;

            //Grunt
            resultTable[GRUNT, GEISHA] = FLIRT;
            resultTable[GRUNT, SENSEI] = CHAT;
            resultTable[GRUNT, NINJA] = WHISPER;
            resultTable[GRUNT, GRUNT] = AGRO;
            resultTable[GRUNT, WARRIOR] = CHAT;
            resultTable[GRUNT, MAN] = INSULT;
            resultTable[GRUNT, WOMAN] = INSULT;

            //Warrior
            resultTable[WARRIOR, GEISHA] = FLIRT;
            resultTable[WARRIOR, SENSEI] = CHAT;
            resultTable[WARRIOR, NINJA] = IGNORE;
            resultTable[WARRIOR, GRUNT] = CHAT;
            resultTable[WARRIOR, WARRIOR] = CHAT;
            resultTable[WARRIOR, MAN] = INSULT;
            resultTable[WARRIOR, WOMAN] = INSULT;

            //Sensei
            resultTable[SENSEI, GEISHA] = SNOB;
            resultTable[SENSEI, SENSEI] = CHAT;
            resultTable[SENSEI, NINJA] = IGNORE;
            resultTable[SENSEI, GRUNT] = CHAT;
            resultTable[SENSEI, WARRIOR] = CHAT;
            resultTable[SENSEI, MAN] = BLESS;
            resultTable[SENSEI, WOMAN] = BLESS;


            //Vil Man
            resultTable[MAN, GEISHA] = BOW;
            resultTable[MAN, SENSEI] = BOW;
            resultTable[MAN, NINJA] = WHISPER;
            resultTable[MAN, GRUNT] = BOW;
            resultTable[MAN, WARRIOR] = BOW;
            resultTable[MAN, MAN] = CHAT;
            resultTable[MAN, WOMAN] = FLIRT;

            //Vil Woman
            resultTable[WOMAN, GEISHA] = BOW;
            resultTable[WOMAN, SENSEI] = BOW;
            resultTable[WOMAN, NINJA] = WHISPER;
            resultTable[WOMAN, GRUNT] = BOW;
            resultTable[WOMAN, WARRIOR] = BOW;
            resultTable[WOMAN, MAN] = FLIRT;
            resultTable[WOMAN, WOMAN] = CHAT;

            
        }
    }


    public void Train()
    {
        int me = typeA.value;
        int them = typeB.value;

        int r = resultTable[me, them];


        //initial result is WHAT they are
        Debug.Log(typenames[me] + " meets " + typenames[them] + " and does " + animParamNames[r]);

        //but what is trained is WHO they are
        NN.retrainWith[0] = inA.value;  //THIS CHARACter        WHO THEY ARE
        NN.retrainWith[1] = inB.value;  //THE OTHER CHARACter   WHO THEY ARE
        NN.retrainWith[2] = r;          //the result
        NN.retrain = true;              //do it (happens in the update, I want to spread the hit over time).

    }

    bool showresult;
    public void Test()
    {
        //we now ignore what TYPE of character it is, and we focus on WHO it is.
        float me = inA.value;
        float them = inB.value;
        NN.question[0] = me;
        NN.question[1] = them;
        NN.ask = true;
        showresult = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(showresult)
        {
            showresult = false;
            Debug.Log(inA.value + " meets " + inB.value + " and does " + NN.answer);


        }
    }
}
