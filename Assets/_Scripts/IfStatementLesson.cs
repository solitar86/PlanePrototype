using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class IfStatementLesson : MonoBehaviour
{

    private float timer = 0f;
    private int number = 0;

    private void Update()
    {
        if(timer < 2f)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0f;
            number = number + 1;
            Debug.Log(number);
        }
    }








}
