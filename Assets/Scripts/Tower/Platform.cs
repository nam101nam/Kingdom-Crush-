using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Platform : MonoBehaviour
{
    public static event Action<Platform> OnPlatformClicked;
    [SerializeField] private LayerMask platformLayerMask;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Update()
    {
        if(Mouse.current.leftButton.wasPressedThisFrame){
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            RaycastHit2D raycastHit = Physics2D.Raycast(worldPoint, Vector2.zero,Mathf.Infinity,platformLayerMask);
            if(raycastHit.collider != null){
                Platform platform = raycastHit.collider.GetComponent<Platform>();
                if(platform != null){
                     OnPlatformClicked?.Invoke(platform);
                    }
            }
        }
    }
}
