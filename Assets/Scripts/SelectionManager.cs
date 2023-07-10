using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    [SerializeField] private Material highlightMaterial;

    // Update is called once per frame
    void Update()
    {
        //create ray using camera as starting point
        //Input.mousePosition is always the center of the screen
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out var hitInfo))
        {
            var selection = hitInfo.transform;
            var selectionRenderer = selection.GetComponent<Renderer>();
            if (selectionRenderer != null)
            {
                if(selection.tag == "Item"){
                    Debug.Log(selection.tag);
                    selectionRenderer.material = highlightMaterial;
                }
            }
        }
    }
}
