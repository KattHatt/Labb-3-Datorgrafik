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

            EntityFactory.CreateModel("models/wolf", new Vector3(0, 0 ,0));
            EntityFactory.CreateCamera(GraphicsDevice);
            EntityFactory.CreateShadowMap();

            // Init all systems
            sm.Init(GraphicsDevice);

            target2D = new RenderTarget2D(GraphicsDevice, 2048, 2048, false, SurfaceFormat.Single, DepthFormat.None);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            shadowShader = Content.Load<Effect>("ShaderLight");
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
        RenderTarget2D target2D;
        Texture2D shadowMap;

        protected override void Draw(GameTime gameTime)
        {
            //Turn off culling so we see both sides of our rendered triangle
            GraphicsDevice.RasterizerState = new RasterizerState
            {
                CullMode = CullMode.None,
                FillMode = FillMode.Solid
            };

            RenderDepthMap();
            Render();

            base.Draw(gameTime);
        }

        private void RenderDepthMap()
        {
            Vector3 lightDirection = shadowShader.Parameters["LightDirection"].GetValueVector3();
            Matrix lightView = Matrix.CreateLookAt(lightDirection, lightDirection * 0.1f, Vector3.Up);
            Matrix lightProjection = Matrix.CreateOrthographic(2048, 2048, 0, 1000);
            shadowShader.Parameters["LightView"].SetValue(lightView);
            shadowShader.Parameters["LightProjection"].SetValue(lightProjection);

            GraphicsDevice.SetRenderTarget(target2D);
            GraphicsDevice.Clear(Color.Black);
            sm.Render(GraphicsDevice, shadowShader, "RenderDepthMap");
            shadowMap = target2D;
        }

        private void Render()
        {
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            sm.Render(GraphicsDevice, shadowShader, "Render");
        }
    }
}
