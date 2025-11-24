
using UnityEngine;

public class CharacterBuilder : MonoBehaviour
{
    //TODO: put these in a common place
     int GEISHA = 0;
     int NINJA = 1;
     int GRUNT = 2;
     int WARRIOR = 3;
     int SENSEI = 4;
     int MAN = 5;
     int WOMAN = 6;
     
     string[] typenames = { "GEISHA", "NINJA", "GRUNT", "WARRIOR","SENSEI", "MAN", "WOMAN" };
     string [] animParamNames = 
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
     
     LUTTest[] LUTs = new LUTTest[9];
     public int meType = 0;
     public int meID = 1;
     
     public GameObject animNetworkTemplate;

     [SerializeField]
     string TypeName;
     [SerializeField]
     Animator animtree;
     [SerializeField]
     string Filename;
     
     public bool trainLUT = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //for convienence 
        string rootName = transform.name;
        animtree = GetComponent<Animator>();
        int curindex = 0;
        
        //hierarch order critical!
        for(int i = 0 ; i < typenames.Length; i++)
        {
            Transform child = transform.GetChild(i);
            //look for the active character in the list
            if (child.gameObject.activeSelf == true)  //this is the one!!
            {
               
                //make this visible
                TypeName = typenames[curindex];
                
                //let's build off of this sub-name
                for (int j = 0; j < animtree.parameterCount; j++)
                {
                    rootName = transform.name + "_" +  child.name + "_" + animtree.parameters[j].name;
                    
                    //make an NN for each anim
                    Debug.Log("rootName: " + rootName);
                    
                    //get the animation network template
                    //make and add one for each animation parameter
                    //based on look up table, train accordingly
                    //save the data
                    GameObject nn = Instantiate(animNetworkTemplate, transform);
                    nn.name = rootName;
                    nn.GetComponent<NN_base>().filename = rootName;
                    AnimParamDriver driver = nn.transform.GetComponent<AnimParamDriver>();
                    driver.animTree = animtree;
                    driver.parmName =  animtree.parameters[j].name;
                    
                    //collect the lok up tables we will use for initial training
                    LUTs[j] = nn.transform.GetComponent<LUTTest>();
                    
                    nn.SetActive(true);

                }    
            }

            curindex++;

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (trainLUT == true)
        {
            //iterate each other character type
            for (int i = 0; i < typenames.Length; i++)
            {
                //and each anim type
                for (int j = 0; j < animParamNames.Length; j++)
                {
                    //LUTs[j].Train(meType,i,meID,10 * (i + 1) );
                    //for now id is type * 10 for testing. Not sure yet if I want to use IDs
                }
            }
            trainLUT = false;
        }
    }
}
