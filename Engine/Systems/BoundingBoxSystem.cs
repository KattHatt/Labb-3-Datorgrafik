using Engine.Components;
using Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Systems
{
    public class BoundingBoxSystem : IRender, ILoad, IInit
    {
        ComponentManager cm = ComponentManager.GetInstance();
        BasicEffect be;

        public void Init(GraphicsDevice gd)
        {
            CameraComponent cam = cm.GetComponentsOfType<CameraComponent>().First().Item2;
            be = new BasicEffect(gd)
            {
                VertexColorEnabled = false,
                TextureEnabled = true
            };
        }

        public void Load(ContentManager content)
        {
            foreach (var (key, mic, bbc) in cm.GetComponentsOfType<ModelInstanceComponent, BoundingBoxComponent>())
            {
                bbc.BoundingBox = CreateBoundingBox(mic.ModelEntityId, mic.Instance);
                CreateBoundingBoxBuffers(bbc);
                CreateBoundingBoxIndices(bbc);
            }
        }

        public void Render(GraphicsDevice gd)
        {
            CameraComponent cam = cm.GetComponentsOfType<CameraComponent>().First().Item2;
            be.View = cam.View;
            be.Projection = cam.Projection;
            foreach (var (key, bb) in cm.GetComponentsOfType<BoundingBoxComponent>())
            {
                if (!bb.Render)
                    continue;

                be.World = Matrix.Identity;

                gd.SetVertexBuffer(bb.Vertices);
                gd.Indices = bb.Indices;

                be.LightingEnabled = false;
                be.TextureEnabled = false;
                be.VertexColorEnabled = true;

                foreach (EffectPass pass in be.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    gd.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0, bb.PrimitiveCount);
                }
            }
        }

        // Creates a boundingbox for a model and its mesh parts
        private BoundingBox CreateBoundingBox(int modelID, Matrix instance)
        {
            BoundingBox result = new BoundingBox();
            Model model = cm.GetComponentForEntity<ModelComponent>(modelID).Model;

            if (model.Bones.Count > 0)
            {
                Matrix[] boneTransforms = new Matrix[model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(boneTransforms);

                foreach (ModelMesh mesh in model.Meshes)
                    foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    {
                        BoundingBox? meshPartBoundingBox = GetBoundingBox(meshPart, boneTransforms[mesh.ParentBone.Index]);
                        if (meshPartBoundingBox != null)
                            result = BoundingBox.CreateMerged(result, meshPartBoundingBox.Value);
                    }
            }
            else
            {
                Matrix transform = cm.GetComponentForEntity<TransformComponent>(modelID).World;

                foreach (ModelMesh mesh in model.Meshes)
                    foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    {
                        BoundingBox? meshPartBoundingBox = GetBoundingBox(meshPart, transform);
                        if (meshPartBoundingBox != null)
                            result = BoundingBox.CreateMerged(result, meshPartBoundingBox.Value);
                    }
            }

            var corners = result.GetCorners();
            corners = (from corner in corners select Vector3.Transform(corner, instance)).ToArray();
            result = BoundingBox.CreateFromPoints(corners);

            return result;
        }

        // Get the boundingbox of a model mesh part
        private BoundingBox? GetBoundingBox(ModelMeshPart meshPart, Matrix transform)
        {
            if (meshPart.VertexBuffer == null)
                return null;

            Vector3[] positions = GetVertexElement(meshPart, VertexElementUsage.Position);
            if (positions == null)
                return null;

            Vector3[] transformedPositions = new Vector3[positions.Length];
            Vector3.Transform(positions, ref transform, transformedPositions);

            return BoundingBox.CreateFromPoints(transformedPositions);
        }

        // Get vertex positions from a model mesh part
        private Vector3[] GetVertexElement(ModelMeshPart meshPart, VertexElementUsage usage)
        {
            VertexDeclaration vd = meshPart.VertexBuffer.VertexDeclaration;
            VertexElement[] elements = vd.GetVertexElements();

            bool elementPredicate(VertexElement ve) => ve.VertexElementUsage == usage && ve.VertexElementFormat == VertexElementFormat.Vector3;
            if (!elements.Any(elementPredicate))
                return null;

            VertexElement element = elements.First(elementPredicate);

            Vector3[] vertexData = new Vector3[meshPart.NumVertices];
            meshPart.VertexBuffer.GetData((meshPart.VertexOffset * vd.VertexStride) + element.Offset,
                vertexData, 0, vertexData.Length, vd.VertexStride);

            return vertexData;
        }

        // Create buffers
        private void CreateBoundingBoxBuffers(BoundingBoxComponent bbc)
        {
            bbc.PrimitiveCount = 24;
            bbc.VertexCount = 48;

            VertexBuffer vertexBuffer = new VertexBuffer(
                bbc.GraphicsDevice,
                typeof(VertexPositionColor),
                bbc.VertexCount,
                BufferUsage.WriteOnly);

            List<VertexPositionColor> vertices = new List<VertexPositionColor>();

            const float ratio = 5.0f;
            Color c = Color.Red;

            Vector3 xOffset = new Vector3((bbc.BoundingBox.Max.X - bbc.BoundingBox.Min.X) / ratio, 0, 0);
            Vector3 yOffset = new Vector3(0, (bbc.BoundingBox.Max.Y - bbc.BoundingBox.Min.Y) / ratio, 0);
            Vector3 zOffset = new Vector3(0, 0, (bbc.BoundingBox.Max.Z - bbc.BoundingBox.Min.Z) / ratio);
            Vector3[] corners = bbc.BoundingBox.GetCorners();

            // Corner 1.
            vertices.Add(new VertexPositionColor(corners[0], c));
            vertices.Add(new VertexPositionColor(corners[0] + xOffset, c));
            vertices.Add(new VertexPositionColor(corners[0], c));
            vertices.Add(new VertexPositionColor(corners[0] - yOffset, c));
            vertices.Add(new VertexPositionColor(corners[0], c));
            vertices.Add(new VertexPositionColor(corners[0] - zOffset, c));

            // Corner 2.
            vertices.Add(new VertexPositionColor(corners[1], c));
            vertices.Add(new VertexPositionColor(corners[1] - xOffset, c));
            vertices.Add(new VertexPositionColor(corners[1], c));
            vertices.Add(new VertexPositionColor(corners[1] - yOffset, c));
            vertices.Add(new VertexPositionColor(corners[1], c));
            vertices.Add(new VertexPositionColor(corners[1] - zOffset, c));

            // Corner 3.
            vertices.Add(new VertexPositionColor(corners[2], c));
            vertices.Add(new VertexPositionColor(corners[2] - xOffset, c));
            vertices.Add(new VertexPositionColor(corners[2], c));
            vertices.Add(new VertexPositionColor(corners[2] + yOffset, c));
            vertices.Add(new VertexPositionColor(corners[2], c));
            vertices.Add(new VertexPositionColor(corners[2] - zOffset, c));

            // Corner 4.
            vertices.Add(new VertexPositionColor(corners[3], c));
            vertices.Add(new VertexPositionColor(corners[3] + xOffset, c));
            vertices.Add(new VertexPositionColor(corners[3], c));
            vertices.Add(new VertexPositionColor(corners[3] + yOffset, c));
            vertices.Add(new VertexPositionColor(corners[3], c));
            vertices.Add(new VertexPositionColor(corners[3] - zOffset, c));

            // Corner 5.
            vertices.Add(new VertexPositionColor(corners[4], c));
            vertices.Add(new VertexPositionColor(corners[4] + xOffset, c));
            vertices.Add(new VertexPositionColor(corners[4], c));
            vertices.Add(new VertexPositionColor(corners[4] - yOffset, c));
            vertices.Add(new VertexPositionColor(corners[4], c));
            vertices.Add(new VertexPositionColor(corners[4] + zOffset, c));

            // Corner 6.
            vertices.Add(new VertexPositionColor(corners[5], c));
            vertices.Add(new VertexPositionColor(corners[5] - xOffset, c));
            vertices.Add(new VertexPositionColor(corners[5], c));
            vertices.Add(new VertexPositionColor(corners[5] - yOffset, c));
            vertices.Add(new VertexPositionColor(corners[5], c));
            vertices.Add(new VertexPositionColor(corners[5] + zOffset, c));

            // Corner 7.
            vertices.Add(new VertexPositionColor(corners[6], c));
            vertices.Add(new VertexPositionColor(corners[6] - xOffset, c));
            vertices.Add(new VertexPositionColor(corners[6], c));
            vertices.Add(new VertexPositionColor(corners[6] + yOffset, c));
            vertices.Add(new VertexPositionColor(corners[6], c));
            vertices.Add(new VertexPositionColor(corners[6] + zOffset, c));

            // Corner 8.
            vertices.Add(new VertexPositionColor(corners[7], c));
            vertices.Add(new VertexPositionColor(corners[7] + xOffset, c));
            vertices.Add(new VertexPositionColor(corners[7], c));
            vertices.Add(new VertexPositionColor(corners[7] + yOffset, c));
            vertices.Add(new VertexPositionColor(corners[7], c));
            vertices.Add(new VertexPositionColor(corners[7] + zOffset, c));

            vertexBuffer.SetData(vertices.ToArray());
            bbc.Vertices = vertexBuffer;
        }

        // Create Indices
        private void CreateBoundingBoxIndices(BoundingBoxComponent bbc)
        {
            IndexBuffer indexBuffer = new IndexBuffer(bbc.GraphicsDevice, IndexElementSize.SixteenBits, bbc.VertexCount,
                BufferUsage.WriteOnly);
            indexBuffer.SetData(Enumerable.Range(0, bbc.VertexCount).Select(i => (short)i).ToArray());
            bbc.Indices = indexBuffer;
        }
    }
}