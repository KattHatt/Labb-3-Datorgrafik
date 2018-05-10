using Engine.Managers;
using Engine.Systems;
using Labb3_Datorgrafik.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Labb3_Datorgrafik
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Labb3 : Game
    {
        GraphicsDeviceManager graphics;

        SystemManager sm = SystemManager.GetInstance();

        public Labb3()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>

        protected override void Initialize()
        {
            
            ComponentManager cm = ComponentManager.GetInstance();

            //Create all systems
            sm.AddSystem(new TransformSystem());
            sm.AddSystem(new CameraSystem());
            sm.AddSystem(new HeightMapSystem());
            sm.AddSystem(new ModelSystem());
            sm.AddSystem(new BoundingBoxSystem());
            sm.AddSystem(new RectangleSystem());
            sm.AddSystem(new SpotLightSystem());
            
            //Create all entities            
            int heightmap = EntityFactory.CreateTerrain(GraphicsDevice, "flatmap", "grass");

            Vector3 corner1 = new Vector3(-155, 270, -287);
            Vector3 corner2 = corner1 + new Vector3(20, 20, 20);
            int cube = EntityFactory.CreateGrassBox(GraphicsDevice, corner1, corner2);

            EntityFactory.CreateCamera(GraphicsDevice);

            EntityFactory.CreateSpotLight();


            // Init all systems
            sm.Init<CameraSystem>(GraphicsDevice);
            sm.Init<HeightMapSystem>(GraphicsDevice);
            sm.Init<BoundingBoxSystem>(GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            sm.Load<HeightMapSystem>(Content);
            //sm.Load<ModelSystem>(Content);
            sm.Load<RectangleSystem>(Content);
            sm.Load<SpotLightSystem>(Content);
        }


        protected override void UnloadContent()
        {
        }

       
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                ButtonState.Pressed || Keyboard.GetState().IsKeyDown(
                Keys.Escape))
                Exit();

            sm.Update<TransformSystem>(gameTime);
            sm.Update<CameraSystem>(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //Turn off culling so we see both sides of our rendered triangle
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            rasterizerState.FillMode = FillMode.Solid;
            GraphicsDevice.RasterizerState = rasterizerState;

            sm.Render<CameraSystem>(GraphicsDevice);
            sm.Render<HeightMapSystem>(GraphicsDevice);
            sm.Render<RectangleSystem>(GraphicsDevice);
            //sm.Render<ModelSystem>(GraphicsDevice);
            sm.Render<SpotLightSystem>(GraphicsDevice);

            base.Draw(gameTime);
        }
    }
}
