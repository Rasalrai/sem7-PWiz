﻿using Interfaces;
using System;

namespace FoodDBEF
{
    public class Producer : Interfaces.IProducer
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
