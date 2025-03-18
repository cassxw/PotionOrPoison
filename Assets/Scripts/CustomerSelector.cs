using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSelector : MonoBehaviour
{
    public Customer[] customers; //array of customers
    public Sprite [] spritesLevelOne;
    public Sprite [] spritesLevelTwo;
    public Sprite [] spritesLevelThree;

    public Sprite [] imposterSprites;

    public int[] numCustomersPerLevel = new int[] {4, 6, 8};
    public int numCustomers;

    public bool currentSpriteIsImposter;

    public Customer[] GetCustomersByLevel(int currentLevel){
        
        numCustomers = numCustomersPerLevel[currentLevel-1];
        customers = new Customer[numCustomers];

        //get sprite array for current level
        Sprite[] selectedSprites = null;
        switch(currentLevel){
            case 1:
                selectedSprites = spritesLevelOne;
                break;
            case 2:
                selectedSprites = spritesLevelTwo;
                break;
            case 3:
                selectedSprites = spritesLevelThree;
                break;
            default:
                Debug.LogError("Invalid level");
                return null;
        }

        //populate customers array according to level
        for(int i=0; i<customers.Length; i++){
            Sprite sprite = selectedSprites[i];
            if(sprite != null){
                currentSpriteIsImposter = Array.Exists(imposterSprites, spr => spr == sprite);
                customers[i] = new Customer {isImposter = currentSpriteIsImposter, image = sprite, level=1};
            }
            else{
                Debug.LogError($"Sprite {selectedSprites[i]} not found in Resources/CustomerSprites/");
            }
        }

        return customers;
    }
}