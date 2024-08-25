using BEPUphysics;
using System;
using static sffair_GreatSpaceRace.Converter;
using static Microsoft.Xna.Framework.MathHelper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/**
 * Game1, Ring, Ship, and Skybox classes contain code references from TomTheTornado
 */
namespace sffair_GreatSpaceRace
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        SpriteFont font;

        private Space space = new Space();
        Ship ship;
        Ring ring1, ring2, ring3, ring4, ring5, ring6, ring7, ring8, ring9;
        Skybox skybox;

        int countRings, totRings, missRings;
        bool playing, end, restart;

        float timer = 0f;
        int curScore = 0;
        int highScore = 0;
        float highScoreTime = 0f;

        private Camera camera { get; set; }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.ApplyChanges();

            Services.AddService<Space>(space);

            skybox = new Skybox(this);

            ship = new Ship(this, new Vector3(0, 0, 700));

            ring1 = new Ring(this, new Vector3(0, 0, -550));
            ring2 = new Ring(this, new Vector3(400, 300, -2150));
            space.Entities[2].WorldTransform = Convert(Matrix.CreateRotationY(TwoPi * -45 / 360f)) * space.Entities[2].WorldTransform;

            ring3 = new Ring(this, new Vector3(2000, 500, -2600));
            space.Entities[3].WorldTransform = Convert(Matrix.CreateRotationY(TwoPi * -90 / 360f)) * space.Entities[3].WorldTransform;

            ring4 = new Ring(this, new Vector3(3600, 0, -2150));
            space.Entities[4].WorldTransform = Convert(Matrix.CreateRotationY(TwoPi * 220 / 360f)) * space.Entities[4].WorldTransform;

            ring5 = new Ring(this, new Vector3(4000, -300, -550));
            space.Entities[5].WorldTransform = Convert(Matrix.CreateRotationY(TwoPi * 180 / 360f)) * space.Entities[5].WorldTransform;

            ring6 = new Ring(this, new Vector3(4000, 0, 550));
            space.Entities[6].WorldTransform = Convert(Matrix.CreateRotationY(TwoPi * 180 / 360f)) * space.Entities[6].WorldTransform;

            ring7 = new Ring(this, new Vector3(3600, 200, 2150));
            space.Entities[7].WorldTransform = Convert(Matrix.CreateRotationY(TwoPi * 160 / 360f)) * space.Entities[7].WorldTransform;

            ring8 = new Ring(this, new Vector3(2000, 0, 2650));
            space.Entities[8].WorldTransform = Convert(Matrix.CreateRotationY(TwoPi * 90 / 360f)) * space.Entities[8].WorldTransform;

            ring9 = new Ring(this, new Vector3(400, -400, 2150));
            space.Entities[9].WorldTransform = Convert(Matrix.CreateRotationY(TwoPi * 80 / 360f)) * space.Entities[9].WorldTransform;

            ring1.active = true;
            ring9.finished = true;

            countRings = 0;
            totRings = 9;
            missRings = 0;

            playing = false;
            end = true;
            restart = true;

            camera = new Camera();

            Services.AddService<Camera>(camera);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("font");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            UpdateInput();
                
            if(!end)
            {
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                LastRing(ring9);
                UpdateRing(ring8, ring9);
                UpdateRing(ring7, ring8);
                UpdateRing(ring6, ring7);
                UpdateRing(ring5, ring6);
                UpdateRing(ring4, ring5);
                UpdateRing(ring3, ring4);
                UpdateRing(ring2, ring3);
                UpdateRing(ring1, ring2);
            }
            else
            {
                if(timer > 0)
                {
                    curScore = (int)Math.Ceiling((2500f / timer) + (countRings * 100f * (30f / timer)));
                }
                if(curScore > highScore)
                {
                    highScore = curScore;
                    highScoreTime = timer;
                }
            }

            Services.GetService<Space>().Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);

            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GraphicsDevice.DepthStencilState = DepthStencilState.None;
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;

            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Rings: " + countRings + " / " + totRings + "  Missed Rings: " + missRings, new Vector2(800, 3), Color.White);
            spriteBatch.DrawString(font, "Current Run Time: " + timer.ToString("0.00") + " seconds", new Vector2(800, 33), Color.White);
            spriteBatch.DrawString(font, "Best Score: " + highScore + "   High Score Time: " + highScoreTime.ToString("0.00") + " seconds", new Vector2(3, 3), Color.White);

            if(end)
            {
                spriteBatch.DrawString(font, "Last Run Score: " + curScore + "  Time: " + timer.ToString("0.00") + " seconds", new Vector2(800, 600), Color.White);
                spriteBatch.DrawString(font, "Press 'R' at anytime to reset, then 'SPACE' to begin", new Vector2(725, 640), Color.White);
            }

            spriteBatch.End();

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }

        /**
         * Code below referenced from TomTheTornado
         */
        protected void UpdateInput()
        {
            KeyboardState currKeyState = Keyboard.GetState();

            float rotSpeed = 1f;

            if (playing)
            {
                if (currKeyState.IsKeyDown(Keys.Up))
                {
                    ship.physicsObject.WorldTransform = Convert(Matrix.CreateRotationX(TwoPi * rotSpeed / 360f)) * ship.physicsObject.WorldTransform;
                }
                if (currKeyState.IsKeyDown(Keys.Down))
                {
                    ship.physicsObject.WorldTransform = Convert(Matrix.CreateRotationX(TwoPi * -rotSpeed / 360f)) * ship.physicsObject.WorldTransform;
                }
                if (currKeyState.IsKeyDown(Keys.Left))
                {
                    ship.physicsObject.WorldTransform = Convert(Matrix.CreateRotationY(TwoPi * rotSpeed / 360f)) * ship.physicsObject.WorldTransform;
                }
                if (currKeyState.IsKeyDown(Keys.Right))
                {
                    ship.physicsObject.WorldTransform = Convert(Matrix.CreateRotationY(TwoPi * -rotSpeed / 360f)) * ship.physicsObject.WorldTransform;
                }

                if (currKeyState.IsKeyDown(Keys.A))
                {
                    ship.physicsObject.WorldTransform = Convert(Matrix.CreateRotationZ(TwoPi * rotSpeed / 360f)) * ship.physicsObject.WorldTransform;
                }
                if (currKeyState.IsKeyDown(Keys.D))
                {
                    ship.physicsObject.WorldTransform = Convert(Matrix.CreateRotationZ(TwoPi * -rotSpeed / 360f)) * ship.physicsObject.WorldTransform;
                }
                if (currKeyState.IsKeyDown(Keys.W))
                {
                    ship.physicsObject.WorldTransform = Convert(Matrix.CreateTranslation(new Vector3(0, 0, -15f))) * ship.physicsObject.WorldTransform;
                }
                if (currKeyState.IsKeyDown(Keys.S))
                {
                    ship.physicsObject.WorldTransform = Convert(Matrix.CreateTranslation(new Vector3(0, 0, 5f))) * ship.physicsObject.WorldTransform;
                }
            }

            if(restart)
            {
                if(currKeyState.IsKeyDown(Keys.Space))
                {
                    restart = false;
                    playing = true;
                    end = false;
                }
            }

            if (currKeyState.IsKeyDown(Keys.R))
            {
                restart = true;
                playing = false;
                end = true;
                curScore = 0;
                timer = 0f;

                countRings = 0;
                totRings = 9;
                missRings = 0;

                ring1.active = true; ring1.passed = false; ring1.missed = false;
                ring2.active = false; ring2.passed = false; ring2.missed = false;
                ring3.active = false; ring3.passed = false; ring3.missed = false;
                ring4.active = false; ring4.passed = false; ring4.missed = false;
                ring5.active = false; ring5.passed = false; ring5.missed = false;
                ring6.active = false; ring6.passed = false; ring6.missed = false;
                ring7.active = false; ring7.passed = false; ring7.missed = false;
                ring8.active = false; ring8.passed = false; ring8.missed = false;
                ring9.active = false; ring9.passed = false; ring9.missed = false;

                ship.physicsObject.Position = Convert(new Vector3(0, 0, 700));
                ship.physicsObject.WorldTransform = Convert(Matrix.Identity);
            }

            camera.CameraPos = Convert(ship.physicsObject.Position) + (Convert(ship.physicsObject.WorldTransform.Backward) * 530f) + Convert(ship.physicsObject.WorldTransform.Up);
            camera.CameraDir = Convert(ship.physicsObject.Position);
            camera.CameraUp = Convert(ship.physicsObject.WorldTransform.Up);
        }

        private void UpdateRing(Ring cur, Ring next)
        {
            float distance = (ship.physicsObject.Position - cur.physicsObject.Position).Length();

            if(distance < 155 && !cur.passed)
            {
                cur.passed = true;
                countRings++;

                if(cur.active)
                if(cur.active)
                {
                    next.active = true;
                    cur.active = false;
                }
                if(cur.missed)
                {
                    cur.missed = false;
                    missRings--;
                }
            }

            if(!cur.passed && !cur.missed && (next.missed || next.passed))
            {
                cur.missed = true;
                missRings++;
            }

            if(cur.passed && cur.active)
            {
                next.active = true;
                cur.active = false;
            }
        }

        private void LastRing(Ring last)
        {
            float distance = (ship.physicsObject.Position - last.physicsObject.Position).Length();

            if(distance < 155)
            {
                last.passed = true;
                countRings++;
                end = true;
            }
        }
    }
}
