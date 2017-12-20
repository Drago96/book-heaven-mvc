using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using BookHeaven.Web.Infrastructure.Mapping;

namespace BookHeaven.Tests
{
    public class AutoMapperInitializer
    {
        private static bool IsInitialized = false;

        private static object mutex = new object();

        public static void Initialize()
        {
            lock (mutex)
            {
                if (!IsInitialized)
                {
                    Mapper.Initialize(cfg =>
                    {
                        cfg.AddProfile<AutoMapperProfile>();

                    });
                    IsInitialized = true;
                }
            }
            
        }
    }
}
