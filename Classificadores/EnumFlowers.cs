using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Classificadores
{
    public enum EnumFlowers
    {
        [Description("Iris-Setosa")]
        Setosa = 0,
        [Description("Iris-versicolor")]
        versicolor = 1,        
        [Description("Iris-virginica")]
        virginica = 2
    }
}
