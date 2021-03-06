﻿// Copyright 2007-2018 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the
// License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the
// specific language governing permissions and limitations under the License.
namespace MassTransit
{
    using System;


    /// <summary>
    /// A timeout, which can be a default (none) or a valid TimeSpan > 0, includes factory methods to make it "cute"
    /// </summary>
    public struct Timeout
    {
        TimeSpan? _timeout;

        Timeout(TimeSpan timeout)
        {
            if (timeout <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeout), "Timeout must be > TimeSpan.Zero");

            _timeout = timeout;
        }

        public bool HasValue => _timeout.HasValue && _timeout.Value > TimeSpan.Zero;

        /// <summary>
        ///
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public TimeSpan Value => _timeout.Value;

        public static Timeout None { get; } = new Timeout();
        public static Timeout Default { get; } = new Timeout(TimeSpan.FromSeconds(30));

        public static implicit operator Timeout(TimeSpan timeout)
        {
            if (timeout <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(timeout), "Must be > TimeSpan.Zero");

            return new Timeout(timeout);
        }

        public static implicit operator Timeout(int milliseconds)
        {
            if (milliseconds <= 0)
                throw new ArgumentOutOfRangeException(nameof(milliseconds), "Must be > 0");

            return After(ms: milliseconds);
        }

        /// <summary>
        /// Create a timeout using optional arguments to build it up
        /// </summary>
        /// <param name="d">days</param>
        /// <param name="h">hours</param>
        /// <param name="m">minutes</param>
        /// <param name="s">seconds</param>
        /// <param name="ms">milliseconds</param>
        /// <returns>The timeout value</returns>
        /// <exception cref="ArgumentException"></exception>
        public static Timeout After(int? d = null, int? h = null, int? m = null, int? s = null, int? ms = null)
        {
            var timeSpan = new TimeSpan(d ?? 0, h ?? 0, m ?? 0, s ?? 0, ms ?? 0);
            if (timeSpan <= TimeSpan.Zero)
                throw new ArgumentException("The timeout must be > 0");

            return new Timeout(timeSpan);
        }
    }
}