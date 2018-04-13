
using Labb1_Datorgrafik.Components;
using Labb2_Datorgrafik;
using System.Collections.Generic;

public class AnimationComponent : IComponent
{
    public bool Animate { get; set; }
    public bool Reverse { get; set; }
    public float Progress { get; set; }
}
