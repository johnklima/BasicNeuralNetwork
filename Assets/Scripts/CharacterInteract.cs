using UnityEngine;
using UnityEngine.UI;
public class CharacterInteract : MonoBehaviour
{

    public Dropdown whoAmI;
    public Dropdown whoAreThey;
    public Transform NPCS;    


    public void Interact()
    {
        Transform ME = NPCS.GetChild(this.whoAmI.value);
        Transform THEM = NPCS.GetChild(this.whoAreThey.value);;

        LUTTest myLUT = ME.GetComponent<LUTTest>();
        string whoAmI = myLUT.transform.name;
        int MyHash = myLUT.myID;
        
        LUTTest theirLUT = THEM.GetComponent<LUTTest>();
        string whoAreThey = theirLUT.transform.name;
        int TheirHash = theirLUT.myID;
        
        //make sure I have my pointers correct
        Debug.Log(whoAmI + " my hash " + MyHash  + " interacts with " + whoAreThey + " their hash" + TheirHash);

        

    }
}
