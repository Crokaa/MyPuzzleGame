using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ColorLayerDB
{
    public static Dictionary<Color, LayerMask> ColorLayers { get; set; } = new Dictionary<Color, LayerMask>
    {
        {
            Color.red,
            GameLayers.instance.RedWallLayer
        },
        {
            Color.blue,
            GameLayers.instance.BlueWallLayer
        },
        {
            Color.green,
            GameLayers.instance.GreenWallLayer
        }
    };
}
