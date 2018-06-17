using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3_Datorgrafik.Tools
{
    public class ShadowEffect : Effect
    {

        private Matrix _world;
        public Matrix World
        {
            get { return _world; }
            set { _world = value; Parameters["World"].SetValue(value); }
        }

        private Matrix _view;
        public Matrix View
        {
            get { return _view; }
            set { _view = value; Parameters["World"].SetValue(value); }
        }

        private Matrix _projection;
        public Matrix Projection
        {
            get { return _projection; }
            set { _projection = value; Parameters["World"].SetValue(value); }
        }

        private Matrix _lightView;
        public Matrix LightView
        {
            get { return _lightView; }
            set { _lightView = value; Parameters["World"].SetValue(value); }
        }

        private Matrix _lightProjection;
        public Matrix LightProjection
        {
            get { return _lightProjection; }
            set { _lightProjection = value; Parameters["World"].SetValue(value); }
        }

/*uniform float3 EyePosition;

uniform float3 DiffuseColor;
uniform float DiffuseIntensity;

        uniform float3 SpecularColor;
uniform float SpecularIntensity;
        uniform float SpecularPower;

        uniform float3 AmbientColor;

uniform float3 LightDirection;

uniform float ShadowPower;

        uniform float FogStart;
        uniform float FogEnd;
        uniform float3 FogColor;
uniform float FogEnabled;*/

        protected ShadowEffect(Effect cloneSource) : base(cloneSource)
        {
        }

        public ShadowEffect(GraphicsDevice graphicsDevice, byte[] effectCode) : base(graphicsDevice, effectCode)
        {
        }

        public ShadowEffect(GraphicsDevice graphicsDevice, byte[] effectCode, int index, int count) : base(graphicsDevice, effectCode, index, count)
        {
        }
    }
}
