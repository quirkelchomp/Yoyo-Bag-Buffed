using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityYoyoBagBuffed.Projectiles
{
    public class YoyoHealProj : ModProjectile
    {
        public override void AI()
        {
            if (projectile.aiStyle == 99 && projectile.owner == Main.myPlayer && Main.player[projectile.owner].active)
            {
                float minHomingDistance = 1200f;
                Player player = Main.player[(int)projectile.ai[0]];
                Vector2 center = new Vector2(projectile.position.X + projectile.width * 0.5f, projectile.position.Y + projectile.height * 0.5f); // Vector is velocity. Velocity is speed in a given direction. Vector2 is (Speed horizontally, Speed vertically).
                float offsetX = player.Center.X - center.X; // OffsetX is the horizontal distance between the player and the projectile
                float offsetY = player.Center.Y - center.Y; // OffsetY is the vertical distance between the player and the projectile
                float distance = (float)Math.Sqrt(offsetX * offsetX + offsetY * offsetY); // The distance between the player and projectile
                if (distance < minHomingDistance && projectile.position.X < player.position.X + player.width && projectile.position.X + projectile.width > player.position.X && projectile.position.Y < player.position.Y + player.height && projectile.position.Y + projectile.height > player.position.Y)
                {
                    if (projectile.owner == Main.myPlayer && !Main.LocalPlayer.moonLeech) // If the healing projectile belongs to you and if you're not afflicted by a health-sapping effect...
                    {
                        int heal = (int)projectile.ai[1];
                        player.HealEffect(heal, false); // ...heal for the amount dictated in "heal"
                        player.statLife += heal;
                        int damage = player.statLifeMax2 - player.statLife;
                        if (heal > damage)
                        {
                            heal = damage;
                            player.AddBuff(BuffID.Heartreach, 18000, false); // For testing purposes only - gives visible indication that the effect has activated
                        }
                        NetMessage.SendData(66, -1, -1, null, ((int)projectile.ai[0]), (float)heal, 0f, 0f, 0, 0, 0);
                    }
                    projectile.Kill();
                }
                distance = 6f / distance;
                offsetX *= distance;
                offsetY *= distance;
                projectile.velocity.X = (projectile.velocity.X * 15f + offsetX) / 16f;
                projectile.velocity.Y = (projectile.velocity.Y * 15f + offsetY) / 16f;
            }
        }
    }
}
