using UnityEngine;
using System.Collections;
using AdvancedInspector;

[AdvancedInspector]
public class Debug_RenameToFullSpritePath : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [Inspect]
    public void InspectorRename()
    {
        SpriteRenderer tempSpriteRenderer = GetComponent<SpriteRenderer>();
        name = tempSpriteRenderer.sprite.texture.name + "_" + tempSpriteRenderer.sprite.name;
    }
}