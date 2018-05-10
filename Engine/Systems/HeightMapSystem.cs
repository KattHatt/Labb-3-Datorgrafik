using Engine.Components;
using Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Systems
{
    public class HeightMapSystem : IRender, ILoad, IInit
    {
        ComponentManager cm = ComponentManager.GetInstance();
        BasicEffect be;

        public void Init(GraphicsDevice gd)
        {
            be = new BasicEffect(gd)
            {
                VertexColorEnabled = false,
                TextureEnabled = true
            };
        }

        public void Render(GraphicsDevice gd)
        {
            CameraComponent cam = cm.GetComponentsOfType<CameraComponent>().First().Item2;
            be.View = cam.View;
            be.Projection = cam.Projection;
            BoundingFrustum frustum = new BoundingFrustum(be.View * be.Projection);

            foreach (var (_, hmc) in cm.GetComponentsOfType<HeightMapComponent>())
            {
                be.VertexColorEnabled = false;
                be.TextureEnabled = true;
                be.Texture = hmc.Texture;
                be.LightingEnabled = false;
                be.CurrentTechnique.Passes[0].Apply();

                for (int i = 0; i < hmc.VertexBuffers.Length; i++)
                {
                    if (frustum.Contains(hmc.BoundingBoxes[i]) == ContainmentType.Disjoint)
                        continue;

                    gd.SetVertexBuffer(hmc.VertexBuffers[i]);
                    gd.Indices = hmc.IndexBuffers[i];
                    gd.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, hmc.IndexBuffers[i].IndexCount / 3);
                }

                if (!hmc.RenderBoundingBoxes)
                    return;

                foreach (var boundingBox in hmc.BoundingBoxes)
                {
                    DebugRender(gd, be, boundingBox);
                }
            }
        }

        private void DebugRender(GraphicsDevice gd, BasicEffect be, BoundingBox box)
        {
            be.VertexColorEnabled = true;
            be.TextureEnabled = false;
            be.CurrentTechnique.Passes[0].Apply();

            Vector3[] corners = box.GetCorners();
            VertexPositionColor[] vertices = (from vertex in corners select new VertexPositionColor(vertex, Color.Green)).ToArray();
            int[] indices =
            {
                0, 1, 1, 2, 2, 3, 3, 0,
                4, 5, 5, 6, 6, 7, 7, 4,
                0, 4, 1, 5, 2, 6, 3, 7,
            };

            gd.DrawUserIndexedPrimitives(PrimitiveType.LineList, vertices, 0, vertices.Length, indices, 0, indices.Length / 2);
        }

        public void Load(ContentManager content)
        {
            foreach(var (_, hmc) in cm.GetComponentsOfType<HeightMapComponent>())
            {
                List<VertexBuffer> vertexBuffers = new List<VertexBuffer>();
                List<IndexBuffer> indexBuffers = new List<IndexBuffer>();
                List<BoundingBox> boundingBoxes = new List<BoundingBox>();
                List<Vector3[]> verticesList = new List<Vector3[]>();
                List<int[]> indicesList = new List<int[]>();

                hmc.HeightMap = content.Load<Texture2D>(hmc.HeightMapFilePath);
                hmc.Texture = content.Load<Texture2D>(hmc.TextureFilePath);
                hmc.Width = hmc.HeightMap.Width;
                hmc.Height = hmc.HeightMap.Height;

                foreach (var heights in Split(hmc))
                {
                    VertexPositionTexture[] vertices = CreateVertices(heights.Value, heights.Key);
                    int[] indices = CreateIndices(heights.Value.GetLength(0), heights.Value.GetLength(1));

                    VertexBuffer vertexBuffer = new VertexBuffer(hmc.GraphicsDevice, VertexPositionTexture.VertexDeclaration, heights.Value.Length, BufferUsage.None);
                    vertexBuffer.SetData(vertices);
                    vertexBuffers.Add(vertexBuffer);
                    verticesList.Add((from vertex in vertices select vertex.Position).ToArray());
                    IndexBuffer indexBuffer = new IndexBuffer(hmc.GraphicsDevice, IndexElementSize.ThirtyTwoBits, heights.Value.Length * 6, BufferUsage.None);
                    indexBuffer.SetData(indices);
                    indexBuffers.Add(indexBuffer);
                    indicesList.Add(indices);
                    BoundingBox boundingBox = CreateBoundingBox(vertices);
                    boundingBoxes.Add(boundingBox);
                }

                hmc.BoundingBox = boundingBoxes[0];
                for (int i = 1; i < boundingBoxes.Count; i++)
                {
                    hmc.BoundingBox = BoundingBox.CreateMerged(hmc.BoundingBox, boundingBoxes[i]);
                }

                hmc.VertexBuffers = vertexBuffers.ToArray();
                hmc.Vertices = verticesList.ToArray();
                hmc.IndexBuffers = indexBuffers.ToArray();
                hmc.Indices = indicesList.ToArray();
                hmc.BoundingBoxes = boundingBoxes.ToArray();
            }
        }

        private Dictionary<Vector3, float[,]> Split(HeightMapComponent hmc)
        {
            Dictionary<Vector3, float[,]> cells = new Dictionary<Vector3, float[,]>();
            Color[] heightMapData = new Color[hmc.Width * hmc.Height];
            hmc.HeightMap.GetData(heightMapData);

            int rows = hmc.Height / 256;
            int cols = hmc.Width / 256;

            for (int z = 0; z < rows; z++)
            {
                for (int x = 0; x < cols; x++)
                {
                    int x2 = MathHelper.Clamp((x + 1) * 256 + 1, 0, hmc.Width);
                    int z2 = MathHelper.Clamp((z + 1) * 256 + 1, 0, hmc.Height);
                    cells.Add(new Vector3(x * 256, 0, z * 256), getCellHeights(x * 256, x2, z * 256, z2));
                }
            }

            return cells;

            float[,] getCellHeights(int x1, int x2, int y1, int y2)
            {
                float[,] heights = new float[x2 - x1, y2 - y1];

                for (int y = y1; y < y2; y++)
                {
                    for (int x = x1; x < x2; x++)
                    {
                        heights[x - x1, y - y1] = heightMapData[y * hmc.Width + x].R;
                    }
                }

                return heights;
            }
        }

        private VertexPositionTexture[] CreateVertices(float[,] heights, Vector3 offset)
        {
            VertexPositionTexture[] vertices = new VertexPositionTexture[heights.Length];

            for (int z = 0; z < heights.GetLength(1); z++)
            {
                for (int x = 0; x < heights.GetLength(0); x++)
                {
                    float u = (float)((x / 16.0) % 1);
                    float v = (float)((z / 16.0) % 1);
                    vertices[z * heights.GetLength(0) + x] = new VertexPositionTexture(new Vector3(x - 500, heights[x, z], z - 500) + offset, new Vector2(u, v));
                }
            }

            return vertices;
        }

        private int[] CreateIndices(int width, int height)
        {
            List<int> indices = new List<int>();
            int q1, q2, q3, q4;

            for (int z = 0; z < height - 1; z++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    // Calculate the four corners
                    q1 = z * width + x;
                    q2 = z * width + x + 1;
                    q3 = (z + 1) * width + x;
                    q4 = (z + 1) * width + x + 1;
                    // Add indices
                    indices.AddRange(new[]
                    {
                        q1, q2, q3, // First triangle
                        q2, q4, q3, // Second triangle
                    });
                }
            }

            return indices.ToArray();
        }

        private BoundingBox CreateBoundingBox(VertexPositionTexture[] vertices)
        {
            return BoundingBox.CreateFromPoints(from vertex in vertices select vertex.Position);
        }

        public void RenderWithEffect(GraphicsDevice gd, Effect ef)
        {
            throw new System.NotImplementedException();
        }
    }
}
