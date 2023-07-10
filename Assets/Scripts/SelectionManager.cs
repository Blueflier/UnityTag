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
    private Material defaultMaterial;


    private Transform _selection;

    // Update is called once per frame
    void Update()
    {
        //if there is something in selection, then set it to its default material
        //TODO: get selection's default material
        if(_selection != null){
            var selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material = defaultMaterial;
        }
        //create ray using camera as starting point
        //Input.mousePosition is always the center of the screen
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out var hitInfo))
        {
            //selection is getting the GameObject
            var selection = hitInfo.transform.Find("Outside");
            if(selection != null){
                var selectionRenderer = selection.GetComponent<Renderer>();
                defaultMaterial = selectionRenderer.material;
                if (selectionRenderer != null)
                {
                    if(selection.tag == "Item"){
                        Debug.Log(selection.tag);
                        selectionRenderer.material = highlightMaterial;
                    }
                    _selection = selection;
                }
            }
        }
    }
}
