﻿namespace Labb3_Datorgrafik.Components
{
    public class NameComponent : IComponent
    {
        public string Name { get; set; }

        public NameComponent()
        {
            Name = "";
        }

        public NameComponent(string name)
        {
            Name = name;
        }
    }
}
