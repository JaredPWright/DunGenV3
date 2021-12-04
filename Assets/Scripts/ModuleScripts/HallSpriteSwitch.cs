using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallSpriteSwitch : MonoBehaviour
{
    private MacroGridStorage macroGridStorage;
    private SetMacroGrid setMacroGrid;
    private SwappableSpriteStorage swappableSpriteStorage;
    private SpriteRenderer spriteRenderer;
    private bool rightAdjacent, leftAdjacent, frontAdjacent, behindAdjacent = false;

    void Start()
    {
        SwitchSprite();
    }

    void SwitchSprite()
    {
        macroGridStorage = GameObject.Find("DungeonMaster").GetComponent<MacroGridStorage>();
        setMacroGrid = GameObject.Find("DungeonMaster").GetComponent<SetMacroGrid>();
        swappableSpriteStorage = GameObject.Find("DungeonMaster").GetComponent<SwappableSpriteStorage>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        foreach(KeyValuePair<Vector3, GameObject> kvp in macroGridStorage.moduleDictionary)
        {
            float moduleRoomX = this.gameObject.transform.position.x;
            float otherRoomX = kvp.Key.x;

            float moduleRoomY = this.gameObject.transform.position.y;
            float otherRoomY = kvp.Key.x;

            if(moduleRoomX + 10.0f == otherRoomX)
            {
                rightAdjacent = true;
            }else if(moduleRoomX - 10.0f == otherRoomX)
            {
                leftAdjacent = true;
            }

            if(moduleRoomY + 10.0f == otherRoomY)
            {
                frontAdjacent = true;
            }else if(moduleRoomY - 10.0f == otherRoomY)
            {
                behindAdjacent = true;
            }
        }

        bool xAdjacent = rightAdjacent || leftAdjacent;
        bool yAdjacent = frontAdjacent || behindAdjacent;

        if(xAdjacent && yAdjacent)
        {
            spriteRenderer.sprite = swappableSpriteStorage.hallSprite;
            
            if(leftAdjacent)
                spriteRenderer.flipX = true;

            if(behindAdjacent)
                spriteRenderer.flipY = true;
        }
    }
}
