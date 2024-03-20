﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AteaScraper.Mediators
{
    public interface IMediator
    {
        Task<object> Send<TRequest>(TRequest request);
    }
}
