using Labb1_Datorgrafik.Managers;
using Labb1_Datorgrafik.Systems;
using Labb1_Datorgrafik.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Labb1_Datorgrafik.Components;

namespace Labb1_Datorgrafik
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Labb1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //CameraComponent cam;

        //BasicEffect for rendering
        BasicEffect basicEffect;

        ComponentManager cm = ComponentManager.GetInstance();
        SystemManager sm = SystemManager.GetInstance();

        public Labb1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>

        Vector3 target = Vector3.Zero;
        Vector3 location = new Vector3(-10, -350, -170);
        Vector3 up = new Vector3(0, 0, 1);

        protected override void Initialize()
        {
            ComponentManager cm = ComponentManager.GetInstance();

            // Camera
            //cam = new Camera(gd);


            //BasicEffect
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.Alpha = 1f;
            //basicEffect.Projection = Matrix.CreatePerspective(10, 7, 0.1f, 10000);
            basicEffect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), graphics.GraphicsDevice.Viewport.Width / graphics.GraphicsDevice.Viewport.Height, .1f, 10000);

            basicEffect.View = Matrix.CreateRotationX(MathHelper.ToRadians(45)) * Matrix.CreateTranslation(location);

            // Want to see the colors of the vertices, this needs to be on
            basicEffect.VertexColorEnabled = true;

            //Lighting requires normal information which VertexPositionColor does not have
            //If you want to use lighting and VPC you need to create a  custom def
            basicEffect.LightingEnabled = false;        

            //Create all systems
            sm.AddSystem(new CameraSystem());
            sm.AddSystem(new HeightMapSystem());

            //Create all entities
            int c = EntityFactory.CreateCamera(GraphicsDevice);
            EntityFactory.CreateHeightMap(GraphicsDevice, "US_Canyon");


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            sm.GetSystem<HeightMapSystem>().Load(Content);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                ButtonState.Pressed || Keyboard.GetState().IsKeyDown(
                Keys.Escape))
                Exit();

            //sm.Update<CameraSystem>(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                /*basicEffect.View *= Matrix.CreateTranslation()
                cam.Position = new Vector3(cam.Position.X - 1f, cam.Position.Y, cam.Position.Z);
                cam.Target = new Vector3(cam.Target.X - 1f, cam.Target.Y, cam.Target.Z);*/
            }
            //if (Keyboard.GetState().IsKeyDown(Keys.Right))
            //{
            //    cam.Position = new Vector3(cam.Position.X + 1f, cam.Position.Y, cam.Position.Z);
            //    cam.Target = new Vector3(cam.Target.X + 1f, cam.Target.Y, cam.Target.Z);
            //}
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                location += new Vector3(0, -1, 0);
                basicEffect.View = Matrix.CreateRotationX(MathHelper.ToRadians(45)) * Matrix.CreateTranslation(location);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                location += new Vector3(0, 1, 0);
                basicEffect.View = Matrix.CreateRotationX(MathHelper.ToRadians(45)) * Matrix.CreateTranslation(location);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                location += new Vector3(1, 0, 0);
                basicEffect.View = Matrix.CreateRotationX(MathHelper.ToRadians(45)) * Matrix.CreateTranslation(location);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                location += new Vector3(-1, 0, 0);
                basicEffect.View = Matrix.CreateRotationX(MathHelper.ToRadians(45)) * Matrix.CreateTranslation(location);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                location += new Vector3(0, 0, 1);
                basicEffect.View = Matrix.CreateRotationX(MathHelper.ToRadians(45)) * Matrix.CreateTranslation(location);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                location += new Vector3(0, 0, -1);
                basicEffect.View = Matrix.CreateRotationX(MathHelper.ToRadians(45)) * Matrix.CreateTranslation(location);
            }
            //if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
            //{
            //    cam.Position = new Vector3(cam.Position.X, cam.Position.Y, cam.Position.Z + 1f);
            //}
            //if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
            //{
            //    cam.Position = new Vector3(cam.Position.X, cam.Position.Y, cam.Position.Z - 1f);
            //}
            //if (Keyboard.GetState().IsKeyDown(Keys.Space))
            //{
            //    orbit = !orbit;
            //}

            //if (orbit)
            //{
            //    Matrix rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(1f));
            //    cam.Position = Vector3.Transform(cam.Position, rotationMatrix);
            //}
            //cam.ViewMatrix = Matrix.CreateLookAt(cam.Position, cam.Target, Vector3.Up);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Turn off culling so we see both sides of our rendered triangle
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            rasterizerState.FillMode = FillMode.Solid;
            GraphicsDevice.RasterizerState = rasterizerState;

            //sm.Render<CameraSystem>(gd, basicEffect);
            sm.Render<HeightMapSystem>(GraphicsDevice, basicEffect);
            
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                //GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 3);

                /*VertexPositionColor[] vertices = new VertexPositionColor[3];
                vertices[0].Position = new Vector3(-0.5f, -0.5f, 0f);
                vertices[0].Color = Color.Red;
                vertices[1].Position = new Vector3(0, 0.5f, 0f);
                vertices[1].Color = Color.Green;
                vertices[2].Position = new Vector3(0.5f, -0.5f, 0f);
                vertices[2].Color = Color.Yellow;
                int[] indices = { 0, 1, 2 };
                GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, 3, indices, 0, 1);*/
            }

            base.Draw(gameTime);
        }
    }
}
