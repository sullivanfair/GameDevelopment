using System.Transactions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace sffair_ComS437IndvProject1
{
    public class Main : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private SpriteFont font;
        private int strokes = 0;

        private Rectangle startLine;
        private Rectangle startLeftWall;
        private Rectangle topHallWay;
        private Rectangle bottomHallWay;
        private Rectangle rightHallWay;
        private Rectangle leftHallWay;
        private Rectangle bottomLeftFinish;
        private Rectangle bottomRightFinish;
        private Rectangle leftFinish;
        private Rectangle rightFinish;
        private Rectangle topFinish;

        private Texture2D slopeTexture;
        private Rectangle downhillSlope;

        private Rectangle wall;
        private Vector2 wallLoc;
        private Vector2 wallVel;
        private bool wallMoveLeft = false;

        private Texture2D texture;
        private Texture2D startTexture;

        private int power = 1;
        private Rectangle arrow;
        private Vector2 arrowLoc;
        private Vector2 arrowOrigin;
        private float arrowAngle = 0;

        private Texture2D golfball;
        private Vector2 golfballLoc;
        private Vector2 golfballVel;

        private Texture2D hole;
        private Rectangle holeRectangle;

        private Texture2D bomb;
        private Rectangle bombRectangle;

        Song golfballHit;
        Song golfballInHole;
        Song bonk;
        Song nuke;

        private Rectangle GetGolfBallRectangle()
        {
            return new Rectangle((int)golfballLoc.X, (int)golfballLoc.Y, 30, 30);
        }
        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        { 
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("strokes");

            texture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            texture.SetData<Color>(new Color[] { Color.Black });

            startTexture = new Texture2D(graphics.GraphicsDevice, 1, 1);
            startTexture.SetData<Color>(new Color[] { Color.CornflowerBlue });

            startLine = new Rectangle(150, 700, 4, 270);
            startLeftWall = new Rectangle(100, 700, 4, 270);
            topHallWay = new Rectangle(100, 700, 900, 4);
            bottomHallWay = new Rectangle(100, 970, 1170, 4);
            rightHallWay = new Rectangle(1270, 400, 4, 574);
            leftHallWay = new Rectangle(1000, 400, 4, 304);
            bottomLeftFinish = new Rectangle(550, 400, 450, 4);
            bottomRightFinish = new Rectangle(1270, 400, 244, 4);
            leftFinish = new Rectangle(550, 110, 4, 290);
            rightFinish = new Rectangle(1510, 110, 4, 290);
            topFinish = new Rectangle(550, 110, 960, 4);

            slopeTexture = Content.Load<Texture2D>("downhill");
            downhillSlope = new Rectangle(1010, 400, 256, 304);

            wallLoc = new Vector2(1135, 260);
            wall = new Rectangle((int)wallLoc.X, (int)wallLoc.Y, 15, 130);
            wallVel = new Vector2((float)1, (float)0);
       
            golfball = Content.Load<Texture2D>("golfball");
            golfballLoc = new Vector2(150, 815);
            golfballVel = new Vector2((float)0, (float)0);

            arrow = new Rectangle(0, 9, 50, 4);
            arrowLoc = new Vector2(golfballLoc.X, golfballLoc.Y);
            arrowOrigin = new Vector2(0, 0);

            hole = Content.Load<Texture2D>("hole");
            holeRectangle = new Rectangle(1360, 210, 70, 70);

            bomb = Content.Load<Texture2D>("bomb");
            bombRectangle = new Rectangle(650, 255, 40, 40);

            golfballHit = Content.Load<Song>("golfballHit");
            golfballInHole = Content.Load<Song>("golfballInHole");
            bonk = Content.Load<Song>("bonk");
            nuke = Content.Load<Song>("nuke");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            arrowLoc = new Vector2(golfballLoc.X + 15, golfballLoc.Y + 15);

            golfballLoc += golfballVel;

            if (startLeftWall.Intersects(GetGolfBallRectangle()))
            {
                MediaPlayer.Play(bonk);
                golfballVel = Vector2.Reflect(golfballVel, Vector2.UnitX);
            }
            if (topHallWay.Intersects(GetGolfBallRectangle()))
            {
                MediaPlayer.Play(bonk);
                golfballVel = Vector2.Reflect(golfballVel, Vector2.UnitY);
            }
            if (bottomHallWay.Intersects(GetGolfBallRectangle()))
            {
                MediaPlayer.Play(bonk);
                golfballVel = Vector2.Reflect(golfballVel, Vector2.UnitY);
            }
            if (rightHallWay.Intersects(GetGolfBallRectangle()))
            {
                MediaPlayer.Play(bonk);
                golfballVel = Vector2.Reflect(golfballVel, Vector2.UnitX);
            }
            if (leftHallWay.Intersects(GetGolfBallRectangle()))
            {
                MediaPlayer.Play(bonk);
                golfballVel = Vector2.Reflect(golfballVel, Vector2.UnitX);
            }
            if (bottomLeftFinish.Intersects(GetGolfBallRectangle()))
            {
                MediaPlayer.Play(bonk);
                golfballVel = Vector2.Reflect(golfballVel, Vector2.UnitY);
            }
            if (bottomRightFinish.Intersects(GetGolfBallRectangle()))
            {
                MediaPlayer.Play(bonk);
                golfballVel = Vector2.Reflect(golfballVel, Vector2.UnitY);
            }
            if (leftFinish.Intersects(GetGolfBallRectangle()))
            {
                MediaPlayer.Play(bonk);
                golfballVel = Vector2.Reflect(golfballVel, Vector2.UnitX);
            }
            if (rightFinish.Intersects(GetGolfBallRectangle()))
            {
                MediaPlayer.Play(bonk);
                golfballVel = Vector2.Reflect(golfballVel, Vector2.UnitX);
            }
            if (topFinish.Intersects(GetGolfBallRectangle()))
            {
                MediaPlayer.Play(bonk);
                golfballVel = Vector2.Reflect(golfballVel, Vector2.UnitY);
            }

            switch (wallLoc.X)
            {
                case 1235:
                    wallMoveLeft = true;
                    break;
                case 1035:
                    wallMoveLeft = false;
                    break;
                default:
                    break;
            }
            if (wallMoveLeft == false)
            {
                wallLoc += wallVel;
            }
            else
            {
                wallLoc -= wallVel;
            }

            wall = new Rectangle((int)wallLoc.X, (int)wallLoc.Y, 15, 130);

            if (wall.Intersects(GetGolfBallRectangle()))
            {
                if(golfballVel.X == 0 && wallMoveLeft)
                {
                    golfballVel.X = -4;
                }
                else if(golfballVel.X == 0 && !wallMoveLeft)
                {
                    golfballVel.X = -4;
                }

                MediaPlayer.Play(bonk);
                golfballVel = Vector2.Reflect(golfballVel, Vector2.UnitX);
            }

            //Keyboard Controls
            if (Keyboard.HasBeenPressed(Keys.Right))
            {
                arrowAngle += MathHelper.ToRadians(9);
            }
            if(Keyboard.HasBeenPressed(Keys.Left))
            {
                arrowAngle -= MathHelper.ToRadians(9);
            }
            if(Keyboard.HasBeenPressed(Keys.Up))
            {
                power++;

                if(power <= 17)
                {
                    arrow.Width += 10;
                }
                if(power > 17)
                {
                    power = 17;
                }
            }
            if(Keyboard.HasBeenPressed(Keys.Down))
            {
                power--;

                if(power >= 1)
                {
                    arrow.Width -= 10;
                }
                if(power < 0)
                {
                    power = 1;
                }
            }
            if(Keyboard.HasBeenPressed(Keys.Space) && golfballVel.X == 0 && golfballVel.Y == 0)
            {
                MediaPlayer.Play(golfballHit);

                golfballVel.X += (float)(System.Math.Cos(arrowAngle) * (0.5 * power));
                golfballVel.Y += (float)(System.Math.Sin(arrowAngle) * (0.5 * power));

                strokes++;
            }

            if(golfballVel.X != 0 || golfballVel.Y != 0 && !downhillSlope.Contains(GetGolfBallRectangle()))
            {
                if(golfballVel.X > 0)
                {
                    golfballVel.X -= 0.015f;
                }
                else if(golfballVel.X < 0)
                {
                    golfballVel.X += 0.015f;
                }

                if(golfballVel.Y > 0)
                {
                    golfballVel.Y -= 0.015f;
                }
                else if(golfballVel.Y < 0)
                {
                    golfballVel.Y += 0.015f;
                }

                if(golfballVel.X > -0.01 && golfballVel.X < 0.01)
                {
                    golfballVel.X = 0;
                }
                if (golfballVel.Y > -0.01 && golfballVel.Y < 0.01)
                {
                    golfballVel.Y = 0;
                }
            }

            if(downhillSlope.Contains(GetGolfBallRectangle()))
            {
                golfballVel.Y -= 0.02f;
            }

            if(bombRectangle.Intersects(GetGolfBallRectangle()))
            {
                MediaPlayer.Play(nuke);

                golfballVel = new Vector2(0, 0);
                golfballLoc = new Vector2(150, 815);
                strokes = 0;
            }

            if (holeRectangle.Contains(GetGolfBallRectangle()) && golfballVel.X < 4 && golfballVel.Y < 4)
            {
                MediaPlayer.Play(golfballInHole);

                golfballVel = new Vector2(0, 0);
                golfballLoc = new Vector2(150, 815);
                strokes = 0;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);

            spriteBatch.Begin();

            spriteBatch.DrawString(font, "Strokes: " + strokes, new Vector2(1600, 800), Color.Black);
            spriteBatch.DrawString(font, "Controls:\nLeft/Right -> +-Angle\nUp/Down -> +-Power\nSpacebar -> Hit", new Vector2(1600, 500), Color.Black);

            spriteBatch.Draw(startTexture, startLine, Color.CornflowerBlue);
            spriteBatch.Draw(texture, startLeftWall, Color.Black);
            spriteBatch.Draw(texture, topHallWay, Color.Black);
            spriteBatch.Draw(texture, bottomHallWay, Color.Black);
            spriteBatch.Draw(texture, rightHallWay, Color.Black);
            spriteBatch.Draw(texture, leftHallWay, Color.Black);
            spriteBatch.Draw(texture, bottomLeftFinish, Color.Black);
            spriteBatch.Draw(texture, bottomRightFinish, Color.Black);
            spriteBatch.Draw(texture, leftFinish, Color.Black);
            spriteBatch.Draw(texture, rightFinish, Color.Black);
            spriteBatch.Draw(texture, topFinish, Color.Black);

            spriteBatch.Draw(slopeTexture, downhillSlope, Color.White);

            spriteBatch.Draw(texture, wall, Color.Black);

            spriteBatch.Draw(hole, holeRectangle, Color.CornflowerBlue);

            spriteBatch.Draw(bomb, bombRectangle, Color.White);

            spriteBatch.Draw(golfball, new Rectangle((int)golfballLoc.X, (int)golfballLoc.Y, 30, 30), Color.White);

            if (golfballVel.X == 0 && golfballVel.Y == 0)
            {
                spriteBatch.Draw(texture, arrowLoc, arrow, Color.White, arrowAngle, arrowOrigin, 1.0f, SpriteEffects.None, 1);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
