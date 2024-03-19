using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class NewBehaviourScript : ScriptableObject {

    public string itemName;
    public string description;
    
    public Sprite icon;

    public void testPrint() {
        Debug.Log("You have touched the item: " + itemName);
        Debug.Log("The description:" + description);
    }
}