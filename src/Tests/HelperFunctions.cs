﻿using GTmetrix;
using GTmetrix.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public static class HelperFunctions
    {
        public static Client CreateWorkingClient()
        {
            var connection = Connection.Create(Settings.ApiKey, Settings.Username);
            return new Client(connection);
        }

        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}