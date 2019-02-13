using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;  

namespace GameJam
{
    
        //public enum LightType
        //{
        //    Point
        //}

        //public abstract class LightManager
        //{
        //    protected float initialPower;

        //    public Vector3 Position { get; set; }
        //    public Vector4 Color; 

        //    public float ActualPower { get; set; }

        //    public float Power
        //    {
        //        get
        //        {
        //            return initialPower;
        //        }
        //        set
        //        {
        //            initialPower = value;
        //            ActualPower = initialPower;
        //        }
        //    }

        //    public int LightDecay { get; set; }

        //    public LightType LightType { get; private set; }

        //    public bool IsEnabled { get; set; }

        //    public Vector3 Direction { get; set; }

        //    protected LightManager(LightType lightType)
        //    {
        //        LightType = lightType;
        //    }

        //    public void EnableLight(bool enabled, float timeToEnable)
        //    {
        //        // If the light is on
        //        IsEnabled = enabled;
        //    }

        //    // Updates the light
        //    public virtual void Update(GameTime gameTime)
        //    {
        //        if (!IsEnabled) return;
        //    }

        //    protected void CopyBaseFields(LightManager light)
        //    {
        //        light.Color = this.Color;
        //        light.IsEnabled = this.IsEnabled;
        //        light.LightDecay = this.LightDecay;
        //        light.LightType = this.LightType;
        //        light.Position = this.Position;
        //        light.Power = this.Power; 
        //    }

        //    public abstract LightManager DeepCopy();
        //}

    }

