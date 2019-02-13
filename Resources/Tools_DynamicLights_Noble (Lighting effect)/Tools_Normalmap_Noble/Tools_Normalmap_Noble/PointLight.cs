using System;
using System.Collections.Generic;
using System.Linq;


namespace Tools_Normalmap_Noble
{
    public class PointLight : LightManager
    {     
            public PointLight() 
                : base (LightType.Point)
            {

            }

            public override LightManager DeepCopy()
            {
                var newLight = new PointLight();
                CopyBaseFields(newLight);

                return newLight;
            }
        }
    }

