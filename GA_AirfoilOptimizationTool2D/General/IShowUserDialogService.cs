﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA_AirfoilOptimizationTool2D.General
{
    interface IShowUserDialogService<TViewModel>
    {
        bool? ShowDialog(TViewModel context);
    }
}