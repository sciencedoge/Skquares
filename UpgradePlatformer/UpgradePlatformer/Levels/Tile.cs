using System;
using System.Collections.Generic;
using System.Text;

namespace UpgradePlatformer.Levels
{
    class Tile
    {
        public TileTexture Kind;
        public float Rotation;
        public TileCollision CollisionKind;

        public Tile(int kind, int rotation, int collision)
        {
            Kind = (TileTexture)kind;
            Rotation = (MathF.PI * 0.5f) * rotation;
            CollisionKind = (TileCollision)collision;
        }
    }

    enum TileTexture
    {
        GRASS = 1,
        PLATFORM,
        DIRT,
        SPIKE,
        LAVA,
    }

    enum TileCollision
    {
        NORMAL = 100,
        PLATFORM,
        GRAVITY_FLIP,
        DAMAGING_TOP,
        DAMAGING_MIDDLE,
    }
}
