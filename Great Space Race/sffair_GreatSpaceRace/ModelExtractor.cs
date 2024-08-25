using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Security.Principal;

namespace sffair_GreatSpaceRace
{
    internal static class ModelExtractor
    {
        public static void GetVerticesAndIndices(Model collisionModel, out BEPUutilities.Vector3[] vertices, out int[] indices)
        {
            Vector3[] tempVertices;

            GetVerticesAndIndices(collisionModel, out tempVertices, out indices);

            vertices = Converter.Convert(tempVertices);
        }

        public static void GetVerticesAndIndices(Model collisionModel, out Vector3[] vertices, out int[] indices)
        {
            var verticesList = new List<Vector3>();
            var indicesList = new List<int>();
            var transforms = new Matrix[collisionModel.Bones.Count];
            collisionModel.CopyAbsoluteBoneTransformsTo(transforms);

            Matrix transform;

            foreach(ModelMesh mesh in collisionModel.Meshes)
            {
                if(mesh.ParentBone != null)
                {
                    transform = transforms[mesh.ParentBone.Index];
                }
                else
                {
                    transform = Matrix.Identity;
                }

                AddMesh(mesh, transform, verticesList, indicesList);
            }

            vertices = verticesList.ToArray();
            indices = indicesList.ToArray();
        }

        public static void AddMesh(ModelMesh collisionModelMesh, Matrix transform, List<Vector3> vertices, IList<int> indices)
        {
            foreach(ModelMeshPart meshPart in collisionModelMesh.MeshParts)
            {
                int startIndex = vertices.Count;
                var meshPartVertices = new Vector3[meshPart.NumVertices];
                int stride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;

                meshPart.VertexBuffer.GetData(
                    meshPart.VertexOffset * stride,
                    meshPartVertices,
                    0,
                    meshPart.NumVertices,
                    stride);

                Vector3.Transform(meshPartVertices, ref transform, meshPartVertices);
                vertices.AddRange(meshPartVertices);

                if (meshPart.IndexBuffer.IndexElementSize == IndexElementSize.ThirtyTwoBits)
                {
                    var meshIndices = new int[meshPart.PrimitiveCount * 3];
                    meshPart.IndexBuffer.GetData(meshPart.StartIndex * 4, meshIndices, 0, meshPart.PrimitiveCount * 3);

                    for(int i = 0; i < meshIndices.Length; i++)
                    {
                        indices.Add(startIndex + meshIndices[i]);
                    }
                }
                else
                { 
                    var meshIndices = new ushort[meshPart.PrimitiveCount * 3];
                    meshPart.IndexBuffer.GetData(meshPart.StartIndex * 2, meshIndices, 0, meshPart.PrimitiveCount * 3);

                    for(int i = 0; i < meshIndices.Length; i++)
                    {
                        indices.Add(startIndex + meshIndices[i]);
                    }

                }
            }
        }
     }
}
