using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionRequest : MonoBehaviour
{
    public PotionType potionType;
    public string request;
    public string[] reasons; //Length of 2: 0: Genuine, 1: Imposter
    public int level;
}
