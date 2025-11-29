using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NeuralNetworkRegression;
using static UnityEngine.ParticleSystem;

//THE BASE NN CLASS 
public class NN_base : MonoBehaviour
{

   
    private NeuralNetwork nn;
    public float[][] trainX;  //training set the inputs to arrive at an output
    public float[]   trainY;
    
    public int curEpochs = 0;
    public int maxEpochs = 1000;   // how many training iterations
    public float lrnRate = 0.01f;  // how quickly  - if divide grads by batSize
    public int batSize = 10;       // batch size

    
    //config for the NN
    public int numIn  = 2;
    public int numHid = 64;
    public int numOut = 1;

    public bool done = false;
    public bool weightsLoaded = false;
    public bool weightsBuilt = false;
    public bool retrain = false;
    public bool ask = false;

    public int[] Xcols = {0, 1 };
    public int[] Ycols = {2};
    public float[] question = {0.1f,0.2f};   //the question, what just played out
    public float answer;                     //what I should do next
    
    public string filename;
    
    public float[] retrainWith = { 0.0f, 0.0f, 0.0f, };
    public int retrainFrom = 0;  // what portion to retrain/overwrite the existing weights
    public int retrainTo = 70;

    // Start is called before the first frame update
    private void Start()
    {
        //filename to save       
        filename = transform.name;
        
        string trainFile = Application.dataPath + "/Data/flat_train.txt";
       
        trainX = Utils.MatLoad(trainFile,
            Xcols, ',', "#");

        trainY = Utils.MatToVec(Utils.MatLoad(trainFile,
            Ycols, ',', "#"));

        
        
        //make new NN
        nn =  new NeuralNetwork(numIn, numHid, numOut, seed: 0);
        
        //test if we have a built weight file
        if (LoadWeights())
            AskQuestion();



    // batch training params
    Debug.Log("Start Done " + transform.name);
       
    }

   
    float timer = -1;
    
    private void LateUpdate()
    {
                
        if(!done && ( !weightsLoaded || !weightsBuilt ))
        {
            Debug.Log("do " + maxEpochs);

            nn.TrainBatch(trainX, trainY, lrnRate,
              batSize, maxEpochs);


        }

        if (!done)
        {
            
            AskQuestion();

            weightsBuilt = true;
            weightsLoaded = true;
            done = true;
            Debug.Log("DONE SAVED WEIGHTS");


            //// save trained model wts            
            string fn = Application.dataPath + "/Data/" + filename + "_wts.txt";
            nn.SaveWeights(fn);   
            





        }

        if(retrain)
        {
            /*
            for (int y = retrainFrom; y < retrainTo; y++)
            {
                trainX[y][0] = retrainWith[0];
                trainX[y][1] = retrainWith[1];
                trainY[y] = retrainWith[2];
                
            }
            */
            
            nn.TrainBatch(trainX, trainY, lrnRate,
             batSize, maxEpochs);

            done = false;
            retrain = false;

        }

        if(ask)
        {
            ask = false;
            AskQuestion();
        }
    }

    public void AskQuestion()
    {
        

        //input data we want to process
        
        float y = nn.ComputeOutput(question);

        Debug.Log(this.transform.name + " first out = " + y);


        answer = y;

    }
    public bool LoadWeights()
    {
        string fn = Application.dataPath + "/Data/" + filename + "_wts.txt";

        if (nn.LoadWeights(fn))
        {
            Debug.Log("Has Weights ");
            weightsLoaded = true;
            weightsBuilt = true;
            done = true;
            return true;
        }
        else 
        {
            Debug.Log("Does Not Have Weights ");
            return false;
        }

      
    }
}
