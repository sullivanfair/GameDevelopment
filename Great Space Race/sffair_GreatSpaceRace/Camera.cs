using Microsoft.Xna.Framework;

namespace sffair_GreatSpaceRace
{
    internal class Camera
    {
        public Vector3 CameraPos { get; set; }

        public Vector3 CameraDir { get; internal set; }

        public Vector3 CameraUp {  get; internal set; }

        public Camera()
        {
            CameraPos = Vector3.Zero;
            CameraDir = Vector3.Forward;
            CameraUp = Vector3.Up;
        }
    }
}
