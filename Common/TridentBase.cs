﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TritonsHydrants.Content.Projectiles;

namespace TritonsHydrants.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class TridentBase : ModProjectile
    {
        private Vector2 MousePos = Vector2.Zero;

        /// <summary>
        /// 
        /// </summary>
        protected virtual float HoldoutRangeMin => 24f;

        /// <summary>
        /// 
        /// </summary>
		protected virtual float HoldoutRangeMax => 96f;

        /// <summary>
        /// 
        /// </summary>
        protected virtual float DistanceSpawnProj => 100;

        /// <summary>
        /// this bool makes the projectile happen one time
        /// </summary>
        private bool isHappen = false;

        public override void OnSpawn(IEntitySource source)
        {
            MousePos = Main.MouseWorld;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            int duration = player.itemAnimationMax;

            player.heldProj = Projectile.whoAmI;

            if (Projectile.timeLeft > duration)
            {
                Projectile.timeLeft = duration;
            }

            Projectile.velocity = Vector2.Normalize(Projectile.velocity);

            float halfDuration = duration * 0.5f;
            float progress;

            if (Projectile.timeLeft < halfDuration)
            {
                progress = Projectile.timeLeft / halfDuration;

                // Spawn aquatic arrow when spear reach your max distance
                if (isHappen is false)
                {
                    Projectile.NewProjectile(new EntitySource_TileBreak(2, 2), Projectile.Center + player.Center.DirectionTo(MousePos) * DistanceSpawnProj, Projectile.velocity * 8f, ModContent.ProjectileType<AquaticArrow>(), Projectile.damage,
                        Projectile.knockBack, Projectile.owner);

                    isHappen = true;
                }
            }
            else
            {
                progress = (duration - Projectile.timeLeft) / halfDuration;
            }

            Projectile.Center = player.MountedCenter + Vector2.SmoothStep(Projectile.velocity * HoldoutRangeMin, Projectile.velocity * HoldoutRangeMax, progress);
        }
    }
}
