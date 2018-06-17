using Engine.Components;
using Engine.Managers;
using Engine.Systems;
using Labb3_Datorgrafik.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Labb3_Datorgrafik
{
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

        RenderTarget2D shadowMap;
        Effect shadowShader;

        protected override void Initialize()
        {
            ComponentManager cm = ComponentManager.GetInstance();
            shadowMap = new RenderTarget2D(GraphicsDevice, 2048, 2048, false, SurfaceFormat.Single, DepthFormat.Depth24Stencil8, 1, RenderTargetUsage.DiscardContents);
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Create all systems
            sm.AddSystem(new TransformSystem());
            sm.AddSystem(new CameraSystem());
            sm.AddSystem(new BoxSystem());
            sm.AddSystem(new RectangleSystem());
            sm.AddSystem(new ModelSystem());

            //Create all entities
            cm.AddEntityWithComponents(new RectangleComponent(new Vector3(0, 0, 0), 4000, 4000));

            Vector3 corner1 = new Vector3(-155, 270, -287);
            Vector3 corner2 = corner1 + new Vector3(20, 20, 20);
            int cube = EntityFactory.CreateGrassBox(GraphicsDevice, corner1, corner2);

            EntityFactory.CreateModel("models/moffett-hangar2", new Vector3(0,0,0));// Vector3.Lerp(corner1, corner2, 0.5f));
            EntityFactory.CreateCamera(GraphicsDevice);

            // Init all systems
            sm.Init(GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            shadowShader = Content.Load<Effect>("Shader");
            sm.Load(Content);
        }
       
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                ButtonState.Pressed || Keyboard.GetState().IsKeyDown(
                Keys.Escape))
                Exit();

            sm.Update(gameTime);

            base.Update(gameTime);
        }

        SpriteBatch spriteBatch;

        protected override void Draw(GameTime gameTime)
        {
            //Turn off culling so we see both sides of our rendered triangle
            GraphicsDevice.RasterizerState = new RasterizerState
            {
                CullMode = CullMode.None,
                FillMode = FillMode.Solid
            };

            /*GraphicsDevice.SetRenderTarget(shadowMap);
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.White, 1.0f, 0);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;
            sm.RenderShadow(GraphicsDevice, shadowShader);

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            sm.Render(GraphicsDevice);*/

            GraphicsDevice.Clear(Color.CornflowerBlue);
            sm.RenderShadow(GraphicsDevice, shadowShader);
            //sm.Render(GraphicsDevice);

            base.Draw(gameTime);
        }
    }
}
