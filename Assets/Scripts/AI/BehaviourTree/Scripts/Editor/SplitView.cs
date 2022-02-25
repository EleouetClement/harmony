using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Harmony.AI {
    public class SplitView : TwoPaneSplitView {
        public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> { }
    }
}