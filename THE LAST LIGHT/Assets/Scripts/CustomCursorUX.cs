using UnityEngine;
using UnityEngine.UI;

public class CustomCursorUX : MonoBehaviour
{
    [Header("Cursor References")]
    public RectTransform cursorRectTransform;
    public Image cursorImage;

    [Header("Theme Colors")]
    public Color normalColor = new Color(1f, 1f, 1f, 0.4f); // Faint, ghostly white
    public Color hoverColor = new Color(0.8f, 0.1f, 0.1f, 0.9f); // Anxious heartbeat red
            
    [Header("Heartbeat Pulse Settings")]
    public float pulseSpeed = 3f;
    public float minSize = 0.8f;
    public float maxSize = 1.3f;

    private bool isHovering = false;
    private Vector3 originalScale;

    void Start()
    {
        // Hide the default operating system cursor
        Cursor.visible = false; 
        
        originalScale = cursorRectTransform.localScale;
        cursorImage.color = normalColor;
    }

    void Update()
    {
        // 1. Smoothly follow the Mouse Position
        Vector2 mousePosition = Input.mousePosition;
        cursorRectTransform.position = mousePosition;

        // 2. Handle the visual state (Normal vs Hovering)
        if (isHovering)
        {
            // Transition to the anxious red color
            cursorImage.color = Color.Lerp(cursorImage.color, hoverColor, Time.deltaTime * 5f);
            
            // Calculate a heartbeat pulse using PingPong (simulates a throbbing heart)
            float pulse = minSize + Mathf.PingPong(Time.time * pulseSpeed, maxSize - minSize);
            cursorRectTransform.localScale = new Vector3(pulse, pulse, 1f);
        }
        else
        {
            // Calm down, return to faint white
            cursorImage.color = Color.Lerp(cursorImage.color, normalColor, Time.deltaTime * 5f);
            cursorRectTransform.localScale = Vector3.Lerp(cursorRectTransform.localScale, originalScale, Time.deltaTime * 10f);
        }
    }

    // We will call these methods when the mouse enters/exits a button
    public void OnHoverEnter() 
    { 
        isHovering = true; 
    }
    
    public void OnHoverExit() 
    { 
        isHovering = false; 
    }
}