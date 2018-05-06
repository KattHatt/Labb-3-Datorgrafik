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
        Effect ambient;

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

            ambient = Content.Load<Effect>("Ambient");


            //Create all systems
            sm.AddSystem(new TransformSystem());
            sm.AddSystem(new CameraSystem());
            sm.AddSystem(new HeightMapSystem());
            sm.AddSystem(new ModelSystem());
            sm.AddSystem(new ModelInstanceSystem());
            sm.AddSystem(new BoundingBoxSystem());
            
            //Create all entities            
            int heightmap = EntityFactory.CreateTerrain(GraphicsDevice, "flatmap", "checkerboard");
            int apa1 = EntityFactory.CreateModel("column", true);
            int apa2 = EntityFactory.CreateModel("roger", true);


            EntityFactory.CreateVeggies(GraphicsDevice, apa1, 200);
            EntityFactory.CreateVeggies(GraphicsDevice, apa2, 200);
            EntityFactory.CreateCamera(GraphicsDevice);


            // Init all systems
            sm.Init<CameraSystem>(GraphicsDevice);
            sm.Init<HeightMapSystem>(GraphicsDevice);
            sm.Init<ModelInstanceSystem>(GraphicsDevice);
            sm.Init<BoundingBoxSystem>(GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            sm.Load<HeightMapSystem>(Content);
            sm.Load<ModelSystem>(Content);
            sm.Load<ModelInstanceSystem>(Content);
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Turn off culling so we see both sides of our rendered triangle
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            rasterizerState.FillMode = FillMode.Solid;
            GraphicsDevice.RasterizerState = rasterizerState;

            sm.Render<CameraSystem>(GraphicsDevice);
            sm.Render<HeightMapSystem>(GraphicsDevice);
            sm.Render<ModelSystem>(GraphicsDevice);
            sm.Render<ModelInstanceSystem>(GraphicsDevice); 

            base.Draw(gameTime);
        }
    }
}
