using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectoidOdyssey
{
    enum MainLayer
    {
        AbsoluteBottom, Background, Main, Overlay, GUI, AbsoluteTop
    }

    struct Layer
    {
        private const int
            MAINCOUNT = 6;

        private const float
            LAYERINTERVAL = 0.00001f,
            MAININTERVAL = 1.0f / MAINCOUNT,
            HALFINTERVAL = MAININTERVAL * 0.5f;

        public float GetDepth => (int)myMainLayer * MAININTERVAL + HALFINTERVAL + LAYERINTERVAL * mySubLayer;

        private MainLayer myMainLayer;
        private int mySubLayer;

        public Layer(MainLayer aMainLayer, int aSubLayer)
        {
            myMainLayer = aMainLayer;
            mySubLayer = aSubLayer;
        }

        public void Set(MainLayer aMainLayer, int aSubLayer)
        {
            myMainLayer = aMainLayer;
            mySubLayer = aSubLayer;
        }

        public static Layer Default => new Layer(MainLayer.Main, 0);
        public static Layer GUI => new Layer(MainLayer.GUI, 0);
    }
}
