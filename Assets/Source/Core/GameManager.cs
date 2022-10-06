﻿using Assets.Source.Core;
using UnityEngine;

namespace DungeonCrawl.Core
{
    /// <summary>
    ///     Loads the initial map and can be used for keeping some important game variables
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private void Start()
        {
            MapLoader.LoadMap("map_1", true);
            MapLoader.LoadMap("map_1", false);
        }
    }
}
