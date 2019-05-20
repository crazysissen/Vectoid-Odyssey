using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCOdyssey
{
    public enum MainLayer
    {
        AbsoluteBottom, Background, Main, Overlay, GUI, AbsoluteTop
    }

    public struct Layer
    {
        private const int
            MAINCOUNT = 6;

        private const float
            LAYERINTERVAL = 0.00001f,
            MAININTERVAL = 1.0f / MAINCOUNT,
            HALFINTERVAL = MAININTERVAL * 0.5f;

        public float GetDepth => (int)AccessMainLayer * MAININTERVAL + HALFINTERVAL + LAYERINTERVAL * AccessSubLayer;
        public MainLayer AccessMainLayer { get; private set; }
        public int AccessSubLayer { get; private set; }

        public Layer(MainLayer aMainLayer, int aSubLayer)
        {
            AccessMainLayer = aMainLayer;
            AccessSubLayer = aSubLayer;
        }

        public void Set(MainLayer aMainLayer, int aSubLayer)
        {
            AccessMainLayer = aMainLayer;
            AccessSubLayer = aSubLayer;
        }

        public static Layer Default => new Layer(MainLayer.Main, 0);
        public static Layer GUI => new Layer(MainLayer.GUI, 0);
    }
}
