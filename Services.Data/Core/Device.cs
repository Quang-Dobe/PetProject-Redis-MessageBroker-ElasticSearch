﻿namespace Services.Data.Core
{
    public class Device : Base
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsAvailable { get; set; }

        public IEnumerable<User>? Users { get; set; }
    }
}
