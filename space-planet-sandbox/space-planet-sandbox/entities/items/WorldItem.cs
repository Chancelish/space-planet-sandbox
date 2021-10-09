using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using space_planet_sandbox.collisiondetection;
using System;
using System.Collections.Generic;
using System.Text;

namespace space_planet_sandbox.entities.items
{
    public abstract class WorldItem : CollidableEntity
    {
        public InventoryItem associatedItem { get; private set; }
        protected HitBox hitBox;

        public abstract void OnCollision(player.PlayerCharacter entity, PlayerInventory inventory);
        
        public override ICollisionMask GetCollisionMask()
        {
            return hitBox;
        }
    }
}
