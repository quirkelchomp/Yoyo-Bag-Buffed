using CalamityYoyoBagBuffed.Items.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalamityYoyoBagBuffed.Projectiles
{
    public class Electron : ModProjectile
    {
        private bool QuantumTunneling = false;
        private static float ElectronSpinIncrement = 0.22f;
        private static int Lifespan = 240;
        private static float ReboundTime = 50f;
        private static int MinPhotonTimer = 13;
        private static int MaxPhotonTimer = 18;
        //private bool text;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            //mod.GetTexture("Projectiles/Electron");
            projectile.width = 34;
            projectile.height = 34;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = QuantumTunneling;
            projectile.penetrate = -1;
            projectile.extraUpdates = 2;
            projectile.timeLeft = Lifespan;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 2;
            projectile.light = 1f;
            projectile.alpha = 100; // 0 is completely opaque, 255 is completely transparent
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Photon.DrawCenteredAndAfterimage(projectile, Color.WhiteSmoke, ProjectileID.Sets.TrailingMode[projectile.type], 1);
            return false;
        }

        internal int YoyoProjectileIndex // We'll set the index of the yoyo projectile to this property name
        {
            get => Main.player[projectile.owner].heldProj; // This retrieves the whoAmI (i.e. index) value of the current instance your currently held projectile, which in this case is the Nucleus Yoyo projectile... 
            set => Main.player[projectile.owner].heldProj = value; // ...and sets it as a value for this class to recognize as the yoyo projectile's whoAmI in Main.Projectile[]. This allows the boomerang projectiles to find the yoyo's position as a return point.
        }

        // Instead of declaring individual variables, such as number0, number1, ..., and number99, you declare one array variable such as numbers and use numbers[0], numbers[1], and ..., numbers[99].
        // The [] specifies the rank of the array. The rank specifies the size of the array. A specific element in an array is accessed by an index.
        // When the array variable is initialized, you can assign values to the array. Array is a reference type, so you need to use the "new" keyword to create an instance of the array (e.g. float[] ai = new float [10];)
        // You can assign values to individual array elements, by using the index number (e.g. ai[10] = 4500.0f)
        // An element of an array is accessed by indexing the array's name. This is done by placing the index of the element within square brackets after the name of the array.
        public override void AI() // ai[0] and ai[1] usually point towards the x and y world coordinate hover point. This means, for projectiles at least, they are essentially used as timers for the animations of the projectile they belong to.
        {
            //if (!this.text)
            //{
            //    this.text = true;
            //    Main.NewText("Electron.YoyoProjectile whoAmI is " + YoyoProjectileIndex, Color.Crimson, false); // Prints a message telling you the value of the yoyo's whoAmI for the current instance of it. Must match the whoAmI from NucleusProjectile!
            //    Main.NewText("Electron is " + projectile, Color.DarkSalmon, false); // Prints a message telling you the projectile's type (ID#), name, active status (t/f), whoAmI, identity, ai0, and uuid.
            //}
            if ((Main.player[projectile.owner].inventory[Main.player[projectile.owner].selectedItem].type == ItemType<NucleusItem>() || Main.player[projectile.owner].counterWeight == ProjectileType<ModCounterweight>()) && projectile.owner == Main.myPlayer)
            {
                drawOffsetX = -11;
                drawOriginOffsetY = -4;
                drawOriginOffsetX = 0f;
                if (projectile.timeLeft == Lifespan) // If this projectile's animation time left equals its set lifespan (i.e its active-time is up), execute the following code.
                {
                    projectile.ai[0] = 0f; // for example, ai[0] being negative makes a yoyo move back towards the player (i.e. when the yoyo is killed). If this projectile's time is up, ai[0] being zero must mean it is simply set as inactive.
                    projectile.ai[1] = GetPhotonDelay(); // Determines the length of time the homing projectile "Photon" stays active for.
                }
                if (Main.rand.Next(3) == 0) // Returns a random non-negative integer from 0 to 3 every tick. If zero (1/4 chance), execute the following code. This block of code creates the dust particles.
                {
                    int type = (Main.rand.Next(5) == 0) ? 262 : 222; // The ? is a conditional-type operator. If Main.rand.Next returns a zero (1/6 chance), set "type" to be the integer 262. Else, if it returns any number from 1 to 5, set "type" to be 222.
                    int alpha = 0;
                    float scale = 1f;
                    if (type == 222)
                    {
                        alpha = 180;
                        scale = 0.6f + Main.rand.NextFloat(0.4f); // Scales the dust size randomly between 0.6 (60%) to 1.0 (default)
                    }
                    float scaleFactor = Main.rand.NextFloat(0.3f, 0.6f); // Sets the amount the dust grows or shrinks in size
                    int dustIndex = Dust.NewDust(projectile.position, projectile.width, projectile.height, type, 0f, 0f, alpha, default, scale); // Assigns dustIndex as the integer that the new dust particles will be indexed as, allowing it to be sorted into the Main.dust array, making it identifiable/accessible by the game.
                    Main.dust[dustIndex].noGravity = true; // Sets whether or not gravity affects the spawned dust particle.
                    Main.dust[dustIndex].velocity = scaleFactor * projectile.velocity; // Sets velocity (i.e. speed & direction) of the dust particle to scale with the velocity of the Electron projectile.
                    //Main.dust[dustIndex].shader = GameShaders.Armor.GetSecondaryShader(31, Main.LocalPlayer); // Applies a cool shader to the dust, changing its color & luminosity.
                }
                base.projectile.ai[0] += 1f;
                base.projectile.ai[1] -= 1f;
                if (base.projectile.ai[0] == ReboundTime) // If this projectile's AI has completed its circuit (50 frames), then...
                {
                    projectile.netUpdate = true; // ...make sure it gets updated for everyone if you're in multiplayer.
                }
                if (base.projectile.ai[0] >= ReboundTime && YoyoProjectileIndex >= 0 && YoyoProjectileIndex < 1001) // If this projectile is still active (AI hasn't run its course yet), run the AI instructions below.
                {
                    //Player player = Main.player[projectile.owner];
                    //int projIndex = player.heldProj;
                    //projIndex = YoyoProjectileIndex; // Set the projectile whoAmI (i.e. Caches the projectile that is of the "projIndex" index for later access)
                    Projectile yoyoProjectile = Main.projectile[YoyoProjectileIndex];
                    Vector2 yoyoCenter = new Vector2(yoyoProjectile.position.X + yoyoProjectile.width * 0.5f, yoyoProjectile.position.Y + yoyoProjectile.height * 0.5f);
                    //Vector2 yoyoCenter = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY); // This technically works, just not preferable because it follows the mouse rather than the yoyo. Also, laggy.
                    float offsetX = yoyoCenter.X - base.projectile.Center.X;
                    float offsetY = yoyoCenter.Y - base.projectile.Center.Y;
                    float distance = (float)Math.Sqrt(offsetX * offsetX + offsetY * offsetY);
                    float reboundMaxDistance = 3000f;
                    if (distance > reboundMaxDistance)
                    {
                        projectile.Kill();
                    }
                    float speed = 16f;
                    float num2 = 2.4f;
                    distance = speed / distance;
                    offsetX *= distance;
                    offsetY *= distance;
                    if (base.projectile.velocity.X < offsetX)
                    {
                        base.projectile.velocity.X = base.projectile.velocity.X + num2;
                        if (base.projectile.velocity.X < 0f && offsetX > 0f)
                        {
                            Projectile projectile = base.projectile;
                            projectile.velocity.X += num2;
                        }
                    }
                    else if (base.projectile.velocity.X > offsetX)
                    {
                        projectile.velocity.X = projectile.velocity.X - num2;
                        if (projectile.velocity.X > 0f && offsetX < 0f)
                        {
                            Projectile projectile2 = projectile;
                            projectile2.velocity.X -= num2;
                        }
                    }
                    if (base.projectile.velocity.Y < offsetY)
                    {
                        projectile.velocity.Y = projectile.velocity.Y + num2;
                        if (projectile.velocity.Y < 0f && offsetY > 0f)
                        {
                            Projectile projectile3 = projectile;
                            projectile3.velocity.Y += num2;
                        }
                    }
                    else if (base.projectile.velocity.Y > offsetY)
                    {
                        projectile.velocity.Y = projectile.velocity.Y - num2;
                        if (projectile.velocity.Y > 0f && offsetY < 0f)
                        {
                            Projectile projectile4 = projectile;
                            projectile4.velocity.Y -= num2;
                        }
                    }
                    if (Main.myPlayer == base.projectile.owner && base.projectile.Hitbox.Intersects(yoyoProjectile.Hitbox))
                    {
                        projectile.Kill();
                    }
                }
                if (base.projectile.ai[1] <= 0f)
                {
                    ElectronPositronAnnihilation(); // Spawns the homing projectile "Photon" from the main projectile "Electron"
                    projectile.ai[1] = GetPhotonDelay();
                }
                float spinDirection = (base.projectile.direction <= 0) ? -1f : 1f; // ? is a conditional operator. If projectile direction is less than or equal to zero, set num6 to be -1. Otherwise, set num6 to be 1.
                base.projectile.rotation += spinDirection * ElectronSpinIncrement;
            }
        }

        private int GetPhotonDelay() // Determines the length of time the newly-spawned homing projectile stays active?
        {
            return Main.rand.Next(MinPhotonTimer, MaxPhotonTimer + 1); // Returns a random integer with a min/max value of 13 & (18 + 1)
        }

        private void ElectronPositronAnnihilation()
        {
            int type = mod.ProjectileType("Photon");
            int damage = projectile.damage / 5; // The origin's base damage is Electron's damage. Photon's damage will be 1/5 of that.
            float knockBack = 3f;
            float ai = (projectile.direction <= 0) ? -1f : 1f;
            float num = 16f;
            float num2 = 0.9f;
            Vector2 value = new Vector2(Main.rand.NextFloat(-num, num), Main.rand.NextFloat(-num, num));
            Vector2 value2 = Main.rand.NextFloat(-num2, num2) * projectile.velocity;
            Vector2 position = projectile.Center + value + value2;
            if (projectile.owner == Main.myPlayer)
            {
                float ai2 = projectile.melee ? 0f : 1f;
                Projectile.NewProjectile(position, Vector2.Zero, type, damage, knockBack, projectile.owner, ai2, ai);
            }
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