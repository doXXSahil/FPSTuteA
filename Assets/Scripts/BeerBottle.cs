using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerBottle : MonoBehaviour
{
   public List<Rigidbody> allParts = new List<Rigidbody>();

    public void shatter()
    {
        foreach(Rigidbody parts in allParts)
        {
            parts.isKinematic = false; 
        }
    }
}
