using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityYoyoBagBuffed.Projectiles
{
    public class NucleusProjectile : ModProjectile
    {
        //private bool text;

        public override void SetStaticDefaults()
        {
            // The following sets are only applicable to yoyo that use aiStyle 99.
            // YoyosLifeTimeMultiplier is how long in seconds the yoyo will stay out before automatically returning to the player. 
            // Vanilla values range from 3f(Wood) to 16f(Chik), and defaults to -1f. Leaving as -1 will make the time infinite.
            ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = -1f;
            // YoyosMaximumRange is the maximum distance the yoyo sleeps away from the player. 
            // Vanilla values range from 130f(Wood) to 400f(Terrarian), and defaults to 200f
            ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 1200f;
            // YoyosTopSpeed is top speed of the yoyo projectile. 
            // Vanilla values range from 9f(Wood) to 17.5f(Terrarian), and defaults to 10f
            ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 17.5f;
        }

        public override void SetDefaults()
        {
            projectile.extraUpdates = 3;
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = 99; // aiStyle 99 is used for all yoyos, and is strongly suggested, as yoyos are extremely difficult without them
            projectile.friendly = true;
            projectile.penetrate = -1; // Do not use projectile.maxPenetrate as it bugs the yoyo out, causing it to retract after just one hit
            projectile.melee = true;
            projectile.scale = 1f;
            projectile.tileCollide = false; ;
            projectile.light = 1f;
            drawOffsetX = -62;
            drawOriginOffsetY = -62;
        }

        //public int nucleusProjectile {get => mod.ProjectileType<NucleusProjectile>();}

        internal int YoyoWhoAmI // We'll set the index of the current projectile to this name. // Note: => is shortened form of {}. So "get =>" would be the same as "get {}"
        {
            get => projectile.whoAmI; // Main.projectile[i].whoAmI; This retrieves the index value of Main.projectile[] for this projectile instance (i.e. when it is active, hence the ai[1]).
            set => projectile.whoAmI = value; // ...and sets YoyoWhoAmI as the retrieved value.
        }

        // **notes for aiStyle 99:**
        // localAI[0] is used for timing up to YoyosLifeTimeMultiplier
        // localAI[1] can be used freely by specific types
        // ai[0] and ai[1] usually point towards the x and y world coordinate hover point
        // ai[0] is -1f once YoyosLifeTimeMultiplier is reached, when the player is stoned/frozen, when the yoyo is too far away, or the player is no longer clicking the shoot button.
        // ai[0] being negative makes the yoyo move back towards the player
        // Any AI method can be used for dust, spawning projectiles, etc specific to your yoyo.
        public override void AI()
        {
            base.AI();
            //if (!this.text)
            //{
            //    this.text = true;
            //    //Main.NewText("Electron nucleusProjectile value is " + nucleusProjectile, Color.Crimson, false); // Prints a message telling you the value of the yoyo's projectileID. For reference only.
            //    Main.NewText("NucleusProjectile projectile.whoAmI is " + projectile.whoAmI, Color.Cyan, false); // Prints a message telling you the value of the yoyo's whoAmI for the current instance of it.
            //    Main.NewText("NucleusProjectile YoyoWhoAmI is " + YoyoWhoAmI, Color.Cyan, false); // Prints a message telling you the value of YoyoWhoAmI. Should match with the yoyo's whoAmI value.
            //}
            YoyoWhoAmI = projectile.whoAmI; // Set the projectile whoAmI
            if (projectile.owner == Main.myPlayer) // If you are the owner of this projectile... (This condition is important for multiplayer compatibility b/c we want it to only affect YOUR projectile)
            {
                projectile.localAI[1] += 1f; // ...add an additional 1/60 second to the timing of the projectile's AI. Many projectiles use timers to delay actions. Typically we use projectile.ai[0] or projectile.ai[1] as those values are synced automatically, but we can also use class fields as well
                if (projectile.localAI[1] >= 6f) // If the projectile's (usage?) timer has been extended to 6/60 (0.1) seconds or beyond... 
                {
                    Vector2 vector = projectile.velocity; // Vector: a quantity possessing both magnitude and direction, represented by an arrow for direction and a length which is the magnitude. Basically dictates projectile VELOCITY, which is SPEED in a given DIRECTION.
                    Vector2 vector2 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101)); // Creates a new velocity for the spawned HomingProj. Main.rand.Next returns a random # (projectile's speed) between -100 & 101 and does it for X (horizontal direction) and Y (vertical direction). Mimics the randomly sprewing projectiles from the Terrarian.
                    vector2.Normalize(); // To normalize a vector, is to take a vector of any length (e.g. speed) and, keeping it pointing in the same direction, change its length, turning it into what is called a unit vector. Since it describes a vector’s direction without regard to its length, it’s useful to have the unit vector readily accessible.
                    vector2 *= Main.rand.Next(10, 41) * 0.5f; // Since vector2 is normalized, this modifies the randomly-spewed projectiles' speed coming off the yoyo, but not their direction. Speed is a random value b/w 10 & 41, then divided in half.
                    if (Main.rand.Next(3) == 0) // Returns a random number between 0 and 3. If 0 (1/4 chance), executes the following code.
                    {
                        vector2 *= 1.4f; // Makes some of the randomly-spewed projectiles go 140% the speed of what they normally would have done.
                    }
                    vector *= 2f; // We'll use the velocity of the yoyo, multiply it by 2...
                    vector += vector2; // ...Then add the velocity of the randomly-spewed projectiles to it.
                    //for (int j = 0; j < 200; j++) // Loop the following instructions while NPC's assigned value is between 0 and 200. Makes the spawned projectiles home-in on targets.
                    //{
                    //    if (Main.npc[j].CanBeChasedBy(this, false)) // If this NPC is programmed to allow for projectiles to home in on it and hurt it...
                    //    {
                    //        float homingStartRange = 200f; // ...then we'll set an arbitrary number to represent distance. In this case, it'll be the minimum distance at which these projectiles will start homing in on a target.
                    //        float npcPositionX = Main.npc[j].position.X + (float)(Main.npc[j].width / 2); // this is the center of the NPC on the x-axis
                    //        float npcPositionY = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2); // this is the center of the NPC on the y-axis
                    //        float distance = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - npcPositionX) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - npcPositionY); // is the distance between the yoyo and the NPC, regardless of directionality
                    //        if (distance < homingStartRange && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[j].position, Main.npc[j].width, Main.npc[j].height)) // So if the distance between the yoyo and NPC is less than 200, and the NPC is programmed to have collision, then...
                    //        { 
                    //            homingStartRange = distance;
                    //            vector.X = npcPositionX;
                    //            vector.Y = npcPositionY;
                    //            vector -= projectile.Center;
                    //            vector.Normalize(); // Makes it so the projectile's speed can be modified without also altering the projectile's direction. (Remember, velocity is speed AND direction)
                    //            vector *= 40f; // multiplies the normalized vector (i.e. calculates new speeds w/o changing the direction) by 40 to get the new velocity of the homing projectile
                    //        }
                    //    }
                    //}
                    vector *= 0.8f;
                    Projectile.NewProjectile(projectile.Center.X - vector.X, projectile.Center.Y - vector.Y, vector.X, vector.Y, mod.ProjectileType<Electron>(), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
                    projectile.localAI[1] = 0f; // If the above IF and FOR conditions aren't satisfied, return the projectile's (usage?) timer back to default
                }
                if (Main.rand.NextBool()) // 50/50 chance of executing the following code.
                {
                    int type = (Main.rand.Next(3) == 0) ? 244 : 246;
                    float scale = 0.8f + Main.rand.NextFloat(0.6f);
                    int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, type, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].velocity = Vector2.Zero;
                    Main.dust[num].scale = scale;
                }
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
            for (int i = 0; i < 2; i++) // This loop works to spawn healing projectiles
            {
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y - 16f, Main.rand.Next(-10, 11) * .25f, Main.rand.Next(-10, -5) * .25f, mod.ProjectileType<YoyoHealProj>(), (int)(projectile.damage * 50f), 0f, projectile.owner);
                Main.projectile[i].tileCollide = false;
            }
            if (Main.rand.NextBool()) // 50/50 chance of inflicting the following buff/debuff to the NPC
            {
                target.AddBuff(BuffID.Ichor, 300, false);
            }
            if (target.type == NPCID.TargetDummy || !target.canGhostHeal)
            {
                return; // If the NPC hit is a Target Dummy or if it cannot GhostHeal, return this method so that it does nothing.
            }
            Player player = Main.player[projectile.owner];
            if (Main.rand.Next(5) == 0) // Returns a non-negative random integer no higher than 5. If it lands on a 0 (1/6 chance), execute the following code.
            {
                player.statLife++;
                player.HealEffect(15, true);
            }
        }
    }
}