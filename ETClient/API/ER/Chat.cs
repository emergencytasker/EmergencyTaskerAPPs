﻿using Plugin.Net.Http;

namespace ETClient.API.ER
{
    public class Chat : IEntityBase
    {
        public int id { get; set; }
        public int eliminado { get; set; }
        public int cliente { get; set; }
        public int trabajador { get; set; }
    }
}