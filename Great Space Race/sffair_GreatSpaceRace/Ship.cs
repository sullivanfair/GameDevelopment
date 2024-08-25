using BEPUphysics;
using static sffair_GreatSpaceRace.Converter;
using static Microsoft.Xna.Framework.MathHelper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace sffair_GreatSpaceRace
{
    internal class Ship : DrawableGameComponent
    {
        private Model model;

        public BEPUphysics.Entities.Prefabs.Sphere physicsObject;

        public Ship(Game game, Vector3 pos) : base(game)
        {
            game.Components.Add(this);
            physicsObject = new BEPUphysics.Entities.Prefabs.Sphere(Convert(pos), 1);
            Game.Services.GetService<Space>().Add(physicsObject);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            model = Game.Content.Load<Model>("models\\lego spaceship");

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            Camera camera = Game.Services.GetService<Camera>();

            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.Alpha = 1f;
                    effect.World = Convert(Convert(Matrix.CreateScale(0.05f)) * physicsObject.WorldTransform);
                    effect.View = Matrix.CreateLookAt(camera.CameraPos, camera.CameraDir, camera.CameraUp);

                    float aspectRatio = Game.GraphicsDevice.Viewport.AspectRatio;
                    float fov = PiOver4;
                    float nearClipPlane = 0.1f;
                    float farClipPlane = 3500f;

                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(fov, aspectRatio, nearClipPlane, farClipPlane);
                }

                mesh.Draw();
            }

            base.Draw(gameTime);
        }
    }
}
