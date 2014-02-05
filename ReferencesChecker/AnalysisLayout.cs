using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphSharp.Controls;
using AssemblyCheck;

namespace ReferencesChecker
{
    public class AnalysisLayout : GraphLayout<AssemblyInfo, ReferenceEdge, AnalysisResult> { }
}
