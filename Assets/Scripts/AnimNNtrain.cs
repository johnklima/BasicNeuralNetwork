using UnityEngine;
using System.IO;



public class AnimNNtrain : MonoBehaviour
{
    public float WhoAmI = 0.0f;
    public float WhoAreThey = 0.0f;
    public float Reaction = 0.0f;
    
   

    public bool retrain = false;
    public int retrainFrom = 0; //maybe one per word? //TODO !!! 
    public int retrainTo = 5;

    public bool ask = false;

    private AnimParamDriver parmDriver;
    private NN_base baseNN;

    private string nodeName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      

        baseNN = GetComponent<NN_base>();

        nodeName = transform.name;
        //build a json for this nodes retrain values
        if (!load())
            save();             //if it didn't find the file, make one

        parmDriver = GetComponent<AnimParamDriver>();

        retrain = false; 
        
    }

    // Update is called once per frame
    void Update()
    {
        if (retrain)
        {

    
            Retrain(WhoAmI, WhoAreThey, Reaction);
            retrain = false;

        }
        if(ask)
        {
            ask = false;
            Ask();

        }
    }

    private void Retrain(float whoami,  float whoarethey, float _answer )
    {

        Debug.Log(" retrain ");
        baseNN.retrainWith[0] = whoami;
        baseNN.retrainWith[1] = whoarethey;
        baseNN.retrainWith[2] = _answer;
        baseNN.retrainFrom = retrainFrom;
        baseNN.retrainTo = retrainTo;

        baseNN.retrain = true;

    }

    public void Ask ()
    {
        baseNN.question[0] = WhoAmI;
        baseNN.question[1] = WhoAreThey;
        baseNN.AskQuestion();

        if (baseNN.answer < 0.001f)
            baseNN.answer = 0;

        if (baseNN.answer > 1)
            baseNN.answer = 1;


        parmDriver.endValue = baseNN.answer;
        parmDriver.startValue = parmDriver.animTree.GetFloat(parmDriver.parmName);
        parmDriver.lerpValue = true;


    }

    private void OnDestroy()
    {        
        save();
    }

    public bool load()
    {
        //load persitant data
        string path = Application.dataPath + "/Data/" + nodeName + ".json";

        if (File.Exists(path))   //just do it
        {
            string loadPlayerData = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(loadPlayerData, this);
            Debug.Log("loaded " + path);

            return true;
        }
        else
        {
            Debug.Log("Just to let you know, there are no save files yet to load." + nodeName);
            return false;
        }

    }

    public void save()
    {
        //string jsonString = JsonUtility.ToJson(parmNames);
        string path = Application.dataPath + "/Data/" + nodeName + ".json";
        Debug.Log("NN sheet saved " + path);

        string saveData = JsonUtility.ToJson(this); // + jsonString;
        File.WriteAllText(path, saveData);


    }



}
