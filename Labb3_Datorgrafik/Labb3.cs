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
        BasicEffect basicEffect;
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

            basicEffect = new BasicEffect(GraphicsDevice)
            {
                Alpha = 1f,
                // Want to see the colors of the vertices, this needs to be on
                VertexColorEnabled = true,
                //Lighting requires normal information which VertexPositionColor does not have
                //If you want to use lighting and VPC you need to create a  custom def
                LightingEnabled = false
            };

            var effect_shadow1 = Content.Load<Effect>("shadow_pass1");
            var effect_ambient1 = Content.Load<Effect>("Ambient");


            //Create all systems
            sm.AddSystem(new TransformSystem());
            sm.AddSystem(new CameraSystem());
            sm.AddSystem(new HeightMapSystem());
            sm.AddSystem(new ModelSystem());
            sm.AddSystem(new RectangleSystem());
            sm.AddSystem(new AnimationSystem());
            sm.AddSystem(new PlayerSystem());
            sm.AddSystem(new ModelInstanceSystem());
            sm.AddSystem(new BoundingBoxSystem());
            
            //Create all entities            
            int heightmap = EntityFactory.CreateTerrain(GraphicsDevice, "flatmap", "checkerboard");
            int apa1 = EntityFactory.CreateModel("column");
            int apa2 = EntityFactory.CreateModel("roger");


            EntityFactory.CreateVeggies(GraphicsDevice, apa1, 200);
            EntityFactory.CreateVeggies(GraphicsDevice, apa2, 200);
            
            EntityFactory.CreateCamera(GraphicsDevice);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            sm.Load<HeightMapSystem>(Content);
            sm.Load<ModelSystem>(Content);
            sm.Load<RectangleSystem>(Content);
            sm.Load<ModelInstanceSystem>(Content);
            sm.Load<BoundingBoxSystem>(Content);
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

            sm.Update<TransformSystem>(gameTime);
            sm.Update<CameraSystem>(gameTime);
            sm.Update<RectangleSystem>(gameTime);
            sm.Update<AnimationSystem>(gameTime);
            sm.Update<PlayerSystem>(gameTime);

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
            rasterizerState.CullMode = CullMode.None;
            rasterizerState.FillMode = FillMode.Solid;
            GraphicsDevice.RasterizerState = rasterizerState;

            sm.Render<CameraSystem>(GraphicsDevice, basicEffect);
            sm.Render<HeightMapSystem>(GraphicsDevice, basicEffect);
            sm.Render<ModelSystem>(GraphicsDevice, basicEffect);
            sm.Render<RectangleSystem>(GraphicsDevice, basicEffect);
            sm.Render<ModelInstanceSystem>(GraphicsDevice, basicEffect);
            sm.Render<BoundingBoxSystem>(GraphicsDevice, basicEffect);

            base.Draw(gameTime);
        }
    }
}
