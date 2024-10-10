using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NumbstersEatRules
{
    public class NumbstersGameState : MonoBehaviour
    {
        // Start is called before the first frame update
        public void EatFromTop(int value)
        {
            switch (value) {
                case 1:
                    Debug.Log("Eating from top: 1");
                    break;
                    
            }
        }

    }
}