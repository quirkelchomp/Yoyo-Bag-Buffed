using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace CalamityYoyoBagBuffed.Projectiles
{
    public class ModCounterweight : ModProjectile
    {
        //private bool text;

        public override void SetDefaults()
        {
            projectile.extraUpdates = 3;
            projectile.width = 12;
            projectile.height = 12;
            projectile.aiStyle = 99;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.melee = true;
            projectile.scale = 1f;
            projectile.counterweight = true;
        }

        public override void AI() // Will only be called if PreAI returns true
        {
            //if (!this.text)
            //{
            //    this.text = true;
            //    Main.NewText("ModCounterweight is " + projectile, Color.DarkSalmon, false); // Prints a message telling you the projectile's type (ID#), name, active status (t/f), whoAmI, identity, ai0, and uuid.
            //}
            if (projectile.owner == Main.myPlayer) // If you are the owner of this projectile... (This condition is important for multiplayer compatibility b/c we want it to only affect YOUR projectile)
            {
                projectile.localAI[1] += 1f; // ...add an additional 1/60 second to the timing of the projectile's AI. Many projectiles use timers to delay actions. Typically we use projectile.ai[0] or projectile.ai[1] as those values are synced automatically, but we can also use class fields as well
                if (projectile.localAI[1] >= 6f) // If the projectile's (usage?) timer has been extended to 6/60 (0.1) seconds or beyond... 
                {
                    Vector2 vector = projectile.velocity; // Vector: a quantity possessing both magnitude and direction, represented by an arrow for direction and a length which is the magnitude. Basically dictates projectile VELOCITY, which is SPEED in a given DIRECTION.
                    Vector2 vector2 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101)); // Creates a new velocity for the spawned HomingProj. Main.rand.Next returns a random # (projectile's speed) between -100 & 101 and does it for X (horizontal direction) and Y (vertical direction). Mimics the randomly sprewing projectiles from the Terrarian.
                    vector2.Normalize(); // To normalize a vector, is to take a vector of any length (e.g. speed) and, keeping it pointing in the same direction, change its length, turning it into what is called a unit vector. Since it describes a vector’s direction without regard to its length, it’s useful to have the unit vector readily accessible.
                    vector2 *= Main.rand.Next(10, 41) * 0.25f; // Since vector2 is normalized, this modifies the randomly-spewed projectiles' speed coming off the yoyo, but not their direction. Speed is a random value b/w 10 & 41, then get 1/4 of that.
                    if (Main.rand.Next(3) == 0) // Returns a random number b/w 0 and 3. If 0 (1/4 chance), executes the following code.
                    {
                        vector2 *= 1.4f; // Makes some of the randomly-spewed projectiles go 140% the speed of what they normally would have done.
                    }
                    vector *= 0.4f; // We'll use the velocity of the yoyo, multiply it by 0.4 to reduce it by 60%...
                    vector += vector2; // ...Then add the velocity of the randomly-spewed projectiles to it.
                    //for (int j = 0; j < 200; j++) // Loop the following instructions while NPC's assigned value is between 0 and 200. Makes the spawned projectiles home-in on targets.
                    //{
                    //    if (Main.npc[j].CanBeChasedBy(this, false)) // If this NPC is programmed to allow for projectiles to home in on it and hurt it...
                    //    {
                    //        float homingStartRange = 200f; // ...then we'll set an arbitrary number to represent distance. In this case, it'll be the minimum distance at which these projectiles will start homing in on a target.
                    //        float npcPositionX = Main.npc[j].position.X + (float)(Main.npc[j].width / 2); // this is the center of the NPC on the x-axis
                    //        float npcPositionY = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2); // this is the center of the NPC on the y-axis
                    //        float distance = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - npcPositionX) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - npcPositionY); // is the distance between the yoyo and the NPC, regardless of directionality
                    //        if (distance < homingStartRange && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[j].position, Main.npc[j].width, Main.npc[j].height)) // So if the distance between the yoyo and NPC is less than 500, and the NPC is programmed to have collision, then...
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
                    Projectile.NewProjectile(projectile.Center.X - vector.X, projectile.Center.Y - vector.Y, vector.X, vector.Y, ProjectileType<Electron>(), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
                    projectile.localAI[1] = 0f; // If the above IF and FOR conditions aren't satisfied, return the projectile's animation timer back to default
                }
            }
        }

        public override void PostAI()
        {
            Player player = Main.player[projectile.owner];
            Item item = player.inventory[player.selectedItem];
            if (projectile.owner == Main.myPlayer && projectile.counterweight)
            {
                ProjectileID.Sets.YoyosMaximumRange[projectile.type] = ProjectileID.Sets.YoyosMaximumRange[item.shoot]; // player.inventory[player.selectedItem].shoot is the "projectile.type" of the currently active item.
            }
        }
    }
}