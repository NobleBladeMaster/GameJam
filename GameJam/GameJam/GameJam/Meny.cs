using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace GameJam
{
    class Meny
    {


        // alla texturer i menyn
        Texture2D titleScreen_bakrund;
        Texture2D startKnapp;
        Texture2D startKnapplight;
        Texture2D exitKnapp;
        Texture2D exitKnapplight;
        Texture2D creditsKnapp;
        Texture2D creditsKnapplight;
        Texture2D helpknapp;
        Texture2D helpknapplight;
        Texture2D Title;
        //platserna på de olika knapparna i menyn

        Vector2 startKnappPos = new Vector2(300, 250);
        Vector2 Exitknappos = new Vector2(300, 700);
        Vector2 credKnappPos = new Vector2(300, 400);
        Vector2 helpKnappPos = new Vector2(300, 550);
        Vector2 titlepos = new Vector2(0, 0);



        enum GameStates
        {
            TitleScreen, Credits, GameOver,howToPlay,
        };




        protected void Update(GameTime gameTime)
        {


            





        }
















    }
}
