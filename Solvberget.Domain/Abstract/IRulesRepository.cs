﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Abstract
{
    public interface IRulesRepository
    {
        IEnumerable<ItemRule> GetItemRules();
    }
}
