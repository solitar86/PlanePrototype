using UnityEngine;
using UnityEngine.InputSystem

public class Plane_2 : MonoBehaviour
{
    private float _forward = 0;

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.upArrowKey.isPressed) _forward = 1f;

    }
}
