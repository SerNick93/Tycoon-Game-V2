using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;

public class ResearchTree : MonoBehaviour
{
    public SO_ResearchTreeData skateTreeData;
    public PopulateResearchPanel[] researchPanels;

    public void Awake()
    {
        int i = 0;

        //Populate the tech trees.
        foreach (SO_NewUnlock newUnlock in skateTreeData.numberOfUnlocksInThisTree)
        {
            if (i < researchPanels.Length)
            {
                //if the item has not been unlocked already.
                if (!newUnlock.isUnlocked)
                {
                    //check to see if it can be.
                    checkIfUnlocked(newUnlock);

                    researchPanels[i].PopulatePanel(newUnlock);

                }

                //if the item has already been unlocked 
                else
                    researchPanels[i].PopulatePanel(newUnlock);
                i++;
            }
            
        }
    }
    //Check to see if the object has yet been unlocked or notS
    private bool checkIfUnlocked(SO_NewUnlock newUnlock)
    {
        bool[] isUnlocked = new bool[newUnlock.prerequisits.Length];

        for (int i = 0; i < newUnlock.prerequisits.Length; i++)
        {
            if (newUnlock.prerequisits[i].isUnlocked)
            {
                Debug.Log(newUnlock.unlockName + " " + newUnlock.prerequisits[i].unlockName);
                isUnlocked[i] = true;
            }
            else
                continue;
        }
        return isUnlocked.All(x => x);
    }
}

