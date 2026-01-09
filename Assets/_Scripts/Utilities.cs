using UnityEngine;
using UnityEngine.EventSystems;

public class Utilities
{
    public static bool IsCursorOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
        
    }
}
