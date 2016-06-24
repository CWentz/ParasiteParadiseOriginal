using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace VirusGame.SpriteClasses
{
    public class TextureManager
    {
        public static Texture2D[] textures = new Texture2D[36];

        public Texture2D Sprites(int number)
        {
            return textures[number];
        }

        public TextureManager(ContentManager cm)
        {
            textures[0] = cm.Load<Texture2D>("Images/Numbers/0");
            textures[1] = cm.Load<Texture2D>("Images/Numbers/1");
            textures[2] = cm.Load<Texture2D>("Images/Numbers/2");
            textures[3] = cm.Load<Texture2D>("Images/Numbers/3");
            textures[4] = cm.Load<Texture2D>("Images/Numbers/4");
            textures[5] = cm.Load<Texture2D>("Images/Numbers/5");
            textures[6] = cm.Load<Texture2D>("Images/Numbers/6");
            textures[7] = cm.Load<Texture2D>("Images/Numbers/7");
            textures[8] = cm.Load<Texture2D>("Images/Numbers/8");
            textures[9] = cm.Load<Texture2D>("Images/Numbers/9");
            textures[10] = cm.Load<Texture2D>("Images/Numbers/colon");
            textures[11] = cm.Load<Texture2D>("Images/Numbers/slash");
            textures[12] = cm.Load<Texture2D>("Test/bloodcell spritesheet");
            textures[13] = cm.Load<Texture2D>("Sprites/spritesheet_helferzelle");
            textures[14] = cm.Load<Texture2D>("Test/enemyspritesheet");
            textures[15] = cm.Load<Texture2D>("Images/monozyt spritesheet Kopie");
            textures[16] = cm.Load<Texture2D>("Sprites/Nervenew");
            textures[17] = cm.Load<Texture2D>("Sprites/Triggernew");
            textures[18] = cm.Load<Texture2D>("Test/vene background");
            textures[19] = cm.Load<Texture2D>("Images/plasmacellspritesheet");
            textures[20] = cm.Load<Texture2D>("Test/tail");
            textures[21] = cm.Load<Texture2D>("Images/collectible2 Kopie");
            textures[22] = cm.Load<Texture2D>("Images/BGs/testEnd");
            textures[23] = cm.Load<Texture2D>("UI/mainMenuAnimation");
            textures[24] = cm.Load<Texture2D>("Images/Exit_light");
            textures[25] = cm.Load<Texture2D>("LoadingMedia/scoreBoard_blubber");
            textures[26] = cm.Load<Texture2D>("Test/ChadSynapse");
            textures[27] = cm.Load<Texture2D>("Sprites/Gefechtspritesheet");
            textures[28] = cm.Load<Texture2D>("Images/valveL");
            textures[29] = cm.Load<Texture2D>("Sprites/Parasite_Animation");
            textures[30] = cm.Load<Texture2D>("Sprites/parattack");
            textures[31] = cm.Load<Texture2D>("Sprites/PlayerTextures/blood_parasite_sheet");
            textures[32] = cm.Load<Texture2D>("Sprites/PlayerTextures/healsheet");
            textures[33] = cm.Load<Texture2D>("Sprites/PlayerTextures/confusesheet");
            textures[34] = cm.Load<Texture2D>("Sprites/PlayerTextures/getcollectible");
            textures[35] = cm.Load<Texture2D>("Test/tempPart");

        }
    }
}
 