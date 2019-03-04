// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Device.I2c;

namespace Iot.Device.TMD2771
{
    /// <summary>
    /// Realtime Clock TMD2771
    /// </summary>
    public class TMD2771 : IDisposable
    {

        public const byte I2cAddress = 0x39;//device slave address is 0x39
        private I2cDevice _sensor = null;

        /// <param name="sensor">I2C Device, like UnixI2cDevice or Windows10I2cDevice</param>
        public TMD2771(I2cDevice sensor)
        {
            _sensor = sensor;
        }

        public void init()
        {
            Span<byte> setData = stackalloc byte[2];

            setData[0] = 0x00;
            setData[1] = 0x03;
            _sensor.Write(setData);
        }

        public int ReadC0()
        {
            Span<byte> data = stackalloc byte[1];

            _sensor.Write(new [] { (byte)0x29 });
            _sensor.Read(data);

            // return data c0
            return data[0];
        }

        public int ReadC1()
        {
            Span<byte> data = stackalloc byte[1];

            _sensor.Write(new[] { (byte)0x2d });
            _sensor.Read(data);

            // return data c0
            return data[0];
        }

        public int ReadId()
        {
            Span<byte> data = stackalloc byte[1];

            _sensor.Write(new[] { (byte)0x25 });
            _sensor.Read(data);

            // return data c0
            return data[0];
        }

        public void Dispose()
        {
            if (_sensor != null)
            {
                _sensor.Dispose();
                _sensor = null;
            }
        }

    }
}
