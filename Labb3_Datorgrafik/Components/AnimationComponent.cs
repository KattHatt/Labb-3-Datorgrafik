﻿namespace Labb3_Datorgrafik.Components
{
    public class AnimationComponent : IComponent
    {
        public bool Animate { get; set; }
        public bool Reverse { get; set; }
        public float Progress { get; set; }
    }
}
