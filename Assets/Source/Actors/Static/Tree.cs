﻿using Assets.Source.Actors.Static;

namespace DungeonCrawl.Actors.Static
{
    public class Tree : Actor
    {
        // in txt : "T"
        public override char MapIcon => 'T';
      
        public override string DefaultSpriteId => "MapTree";
        public override string DefaultName => "Tree";
    }
}
