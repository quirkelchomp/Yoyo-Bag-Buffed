using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityYoyoBagBuffed.Projectiles
{
    public class Photon : ModProjectile
    {
        private static int Lifetime = 90;
        private static float MaxRotationSpeed = 0.25f;
        private static float MaxSpeed = 22f;
        private static float HomingStartRange = 600f;
        private static float HomingBreakRange = 1000f;
        private static float HomingBonusRangeCap = 200f;
        private static float BaseHomingFactor = 1.6f;
        private static float MaxHomingFactor = 6.6f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.penetrate = 4;
            projectile.extraUpdates = 1;
            projectile.timeLeft = Lifetime;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 8;
            projectile.light = 1f;
            projectile.alpha = 150; // 0 is completely opaque, 255 is completely transparent
        }

        public override void AI()
        {
            drawOffsetX = -10;
            drawOriginOffsetY = 0;
            drawOriginOffsetX = 0f;
            if (projectile.timeLeft == Lifetime)
            {
                projectile.ai[0] = 0f;
                SpawnDust();
            }
            float num = projectile.ai[1];
            projectile.direction = ((num <= 0f) ? 1 : -1);
            projectile.spriteDirection = projectile.direction;
            projectile.rotation += num * MaxRotationSpeed;
            float num2 = 0.0166f * num;
            projectile.ai[1] -= num2;
            if (projectile.timeLeft < 15)
            {
                projectile.scale *= 0.92f;
            }
            HomingAI();
        }

        private void HomingAI()
        {
            int num = (int)projectile.ai[0] - 1;
            if (num < 0)
            {
                num = AcquireTarget();
            }
            projectile.ai[0] = num + 1f;
            if (num < 0)
            {
                projectile.velocity *= 0.94f;
                return;
            }
            NPC npc = Main.npc[num];
            float offsetX = projectile.Center.X - npc.Center.X;
            float offsetY = projectile.Center.Y - npc.Center.Y;
            float distance = (float)Math.Sqrt(offsetX * offsetX + offsetY * offsetY);
            if (distance > HomingBreakRange)
            {
                projectile.ai[0] = 0f;
                return;
            }
            float scaleFactor = CalcHomingFactor(distance);
            Vector2 vector = npc.Center - projectile.Center;
            vector = vector.SafeNormalize(Vector2.Zero);
            vector *= scaleFactor;
            Vector2 vector2 = projectile.velocity += vector;
            if (vector2.Length() >= MaxSpeed)
            {
                vector2 = vector2.SafeNormalize(Vector2.Zero);
                vector2 *= MaxSpeed;
            }
            projectile.velocity = vector2;
        }

        private int AcquireTarget()
        {
            bool flag = false;
            int result = -1;
            float homingStartRange = HomingStartRange;
            for (int npcIndex = 0; npcIndex < 200; npcIndex++)
            {
                NPC npc = Main.npc[npcIndex];
                if (npc.active && npc.type != 488 && (!flag || npc.boss) && npc.CanBeChasedBy(projectile, false))
                {
                    float offsetX = projectile.Center.X - npc.Center.X;
                    float offsetY = projectile.Center.Y - npc.Center.Y;
                    float distance = (float)Math.Sqrt(offsetX * offsetX + offsetY * offsetY);
                    if (distance < homingStartRange)
                    {
                        if (npc.boss)
                        {
                            flag = true;
                        }
                        homingStartRange = distance;
                        result = npcIndex;
                    }
                }
            }
            return result;
        }

        private float CalcHomingFactor(float dist)
        {
            float baseHomingFactor = BaseHomingFactor;
            float num = (MaxHomingFactor - BaseHomingFactor) * (1f - dist / HomingBonusRangeCap);
            if (num < 0f)
            {
                num = 0f;
            }
            return baseHomingFactor + num;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Photon.DrawCenteredAndAfterimage(projectile, lightColor, ProjectileID.Sets.TrailingMode[projectile.type], 1);
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            float x = projectile.width / 2f;
            float y = projectile.height / 2f;
            SpriteEffects effects = SpriteEffects.None;
            if (projectile.spriteDirection == -1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 origin = new Vector2(x, y);
            spriteBatch.Draw(mod.GetTexture("Projectiles/AntiPhoton"), projectile.Center - Main.screenPosition, null, default, projectile.rotation, origin, projectile.scale, effects, 100f);
        }

        public override void Kill(int timeLeft)
        {
            SpawnDust();
        }

        private void SpawnDust()
        {
            int num = Main.rand.Next(3, 6);
            Vector2 position = projectile.position;
            for (int i = 0; i < num; i++)
            {
                int type = 247; // Dust ID
                float scale = 0.6f + Main.rand.NextFloat(0.4f);
                int dustIndex = Dust.NewDust(position, projectile.width, projectile.height, type, 0f, 0f, 100, Color.White, scale);
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].velocity *= 3f;
                //Main.dust[dustIndex].shader = GameShaders.Armor.GetSecondaryShader(68, Main.LocalPlayer);
            }
        }

        public static void DrawCenteredAndAfterimage(Projectile projectile, Color lightColor, int trailingMode, int afterimageCounter)
        {
            Texture2D texture2D = Main.projectileTexture[projectile.type];
            int num = texture2D.Height / Main.projFrames[projectile.type];
            int y = num * projectile.frame;
            Rectangle rectangle = new Rectangle(0, y, texture2D.Width, num);
            SpriteEffects effects = SpriteEffects.None;
            if (projectile.spriteDirection == -1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            if (Lighting.NotRetro)
            {
                if (trailingMode != 0)
                {
                    if (trailingMode == 1)
                    {
                        Color color = Lighting.GetColor((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f));
                        int num2 = 8;
                        for (int i = 1; i < num2; i += afterimageCounter)
                        {
                            Color color2 = color;
                            color2 = projectile.GetAlpha(color2);
                            float num3 = num2 - i;
                            color2 *= num3 / (ProjectileID.Sets.TrailCacheLength[projectile.type] * 1.5f);
                            Main.spriteBatch.Draw(texture2D, projectile.oldPos[i] + projectile.Size / 2f - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Rectangle?(rectangle), color2, projectile.rotation, rectangle.Size() / 2f, projectile.scale, effects, 0f);
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < projectile.oldPos.Length; j++)
                    {
                        Vector2 position = projectile.oldPos[j] + projectile.Size / 2f - Main.screenPosition + new Vector2(0f, projectile.gfxOffY);
                        Color color3 = projectile.GetAlpha(lightColor) * ((projectile.oldPos.Length - j) / (float)projectile.oldPos.Length);
                        Main.spriteBatch.Draw(texture2D, position, new Rectangle?(rectangle), color3, projectile.rotation, rectangle.Size() / 2f, projectile.scale, effects, 0f);
                    }
                }
            }
            Main.spriteBatch.Draw(texture2D, projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY), new Rectangle?(new Rectangle(0, y, texture2D.Width, num)), projectile.GetAlpha(lightColor), projectile.rotation, new Vector2(texture2D.Width / 2f, num / 2f), projectile.scale, effects, 0f);
        }

        public override void OnHitPvp(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Stoned, 10000, true);
            target.AddBuff(BuffID.Confused, 0, false);
            Player player = Main.player[projectile.owner];
            if (Main.rand.Next(5) == 0)
            {
                player.statLife++;
                player.HealEffect(500, true);
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.NextBool())
            {
                target.AddBuff(BuffID.Ichor, 300, false);
            }
            if (target.type == NPCID.TargetDummy || !target.canGhostHeal)
            {
                return;
            }
            Player player = Main.player[projectile.owner];
            if (Main.rand.Next(5) == 0)
            {
                player.statLife++;
                player.HealEffect(5, true);
            }
        }
    }
}
