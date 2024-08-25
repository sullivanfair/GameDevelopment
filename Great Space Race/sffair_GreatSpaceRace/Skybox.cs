using static sffair_GreatSpaceRace.Converter;
using static Microsoft.Xna.Framework.MathHelper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace sffair_GreatSpaceRace
{
    internal class Skybox : DrawableGameComponent
    {
        private Model model;
        private Texture2D modelTexture;

        public Skybox(Game game) : base(game)
        {
            game.Components.Add(this);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            modelTexture = Game.Content.Load<Texture2D>("textures\\space");
            model = Game.Content.Load<Model>("models\\sphere");

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            Camera camera = Game.Services.GetService<Camera>();
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rs;

            foreach (var mesh in model.Meshes)
            {
                foreach(BasicEffect effect in mesh.Effects)
                {
                    effect.Texture = modelTexture;
                    effect.Alpha = 1.0f;
                    effect.World = Convert(Convert(Matrix.CreateScale(6500.0f)));
                    effect.View = Matrix.CreateLookAt(camera.CameraPos, camera.CameraDir, camera.CameraUp);

                    float aspectRaio = Game.GraphicsDevice.Viewport.AspectRatio;
                    float fov = PiOver4;
                    float nearClipPlane = 0.1f;
                    float farClipPlane = 13000;

                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(fov, aspectRaio, nearClipPlane, farClipPlane);
                }

                mesh.Draw();
            }

            base.Draw(gameTime);
        }
    }
}
