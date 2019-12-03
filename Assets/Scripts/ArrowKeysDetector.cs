using UnityEngine;

public class ArrowKeysDetector : MonoBehaviour, IInputDetector
{
    public InputDirection? DetectInputDirection()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow))
            return InputDirection.Top;
        if (Input.GetKeyUp(KeyCode.DownArrow))
            return InputDirection.Bottom;
        if (Input.GetKeyUp(KeyCode.RightArrow))
            return InputDirection.Right;
        if (Input.GetKeyUp(KeyCode.LeftArrow))
            return InputDirection.Left;
        return null;
    }
}