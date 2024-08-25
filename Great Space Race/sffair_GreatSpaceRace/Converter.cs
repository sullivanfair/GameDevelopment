using Microsoft.Xna.Framework;

namespace sffair_GreatSpaceRace
{
    internal static class Converter
    {
        public static Vector3 Convert(BEPUutilities.Vector3 bepuVector)
        {
            Vector3 vector3;

            vector3.X = bepuVector.X;
            vector3.Y = bepuVector.Y;
            vector3.Z = bepuVector.Z;

            return vector3;
        }

        public static BEPUutilities.Vector3 Convert(Vector3 xnaVector)
        {
            BEPUutilities.Vector3 vector3;

            vector3.X = xnaVector.X;
            vector3.Y = xnaVector.Y;
            vector3.Z = xnaVector.Z;

            return vector3;
        }

        public static Matrix Convert(BEPUutilities.Matrix matrix)
        {
            Matrix convertedMat;

            Convert(ref matrix, out convertedMat);

            return convertedMat;
        }

        public static BEPUutilities.Matrix Convert(Matrix matrix)
        {
            BEPUutilities.Matrix convertedMat;

            Convert(ref matrix, out convertedMat);

            return convertedMat;
        }

        public static void Convert(ref BEPUutilities.Matrix bepuMatrix, out Matrix xnaMatrix)
        {
            xnaMatrix.M11 = bepuMatrix.M11;
            xnaMatrix.M12 = bepuMatrix.M12;
            xnaMatrix.M13 = bepuMatrix.M13;
            xnaMatrix.M14 = bepuMatrix.M14;

            xnaMatrix.M21 = bepuMatrix.M21;
            xnaMatrix.M22 = bepuMatrix.M22;
            xnaMatrix.M23 = bepuMatrix.M23;
            xnaMatrix.M24 = bepuMatrix.M24;

            xnaMatrix.M31 = bepuMatrix.M31;
            xnaMatrix.M32 = bepuMatrix.M32;
            xnaMatrix.M33 = bepuMatrix.M33;
            xnaMatrix.M34 = bepuMatrix.M34;

            xnaMatrix.M41 = bepuMatrix.M41;
            xnaMatrix.M42 = bepuMatrix.M42;
            xnaMatrix.M43 = bepuMatrix.M43;
            xnaMatrix.M44 = bepuMatrix.M44;
        }

        public static void Convert(ref Matrix matrix, out BEPUutilities.Matrix bepuMatrix)
        {
            bepuMatrix.M11 = matrix.M11;
            bepuMatrix.M12 = matrix.M12;
            bepuMatrix.M13 = matrix.M13;
            bepuMatrix.M14 = matrix.M14;

            bepuMatrix.M21 = matrix.M21;
            bepuMatrix.M22 = matrix.M22;
            bepuMatrix.M23 = matrix.M23;
            bepuMatrix.M24 = matrix.M24;

            bepuMatrix.M31 = matrix.M31;
            bepuMatrix.M32 = matrix.M32;
            bepuMatrix.M33 = matrix.M33;
            bepuMatrix.M34 = matrix.M34;

            bepuMatrix.M41 = matrix.M41;
            bepuMatrix.M42 = matrix.M42;
            bepuMatrix.M43 = matrix.M43;
            bepuMatrix.M44 = matrix.M44;
        }
    }
}
