// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Iot.Device.TMD2771
{
    /// <summary>
    /// Register of TMD2771
    /// </summary>
    internal enum Register : byte
    {
        ENABLE = 0x00,
        C0_DATAL = 0x14,
        C0_DATAH = 0x15,
        C1_DATAL = 0x16,
        C1_DATAH = 0x17
    }
}
