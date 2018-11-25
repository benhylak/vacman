using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class NumberDisplay : MonoBehaviour
{
    public List<GameObject> numbers; 

    public void SetNumber(int num)
    {
        GameObject selectedNum = null;

        if(num < numbers.Count)
        {
            selectedNum = numbers.ElementAt(num);
            numbers.ForEach(x => x.SetActive(x == selectedNum));       
        }
        else Debug.Log("Input to NumberDisplay is Out of Range");
    }
}