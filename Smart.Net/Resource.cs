/*
Copyright (c) 2017, Llewellyn Kruger
All rights reserved.
Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS “AS IS” AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE. 
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace Simplified.IO
{
    public sealed class Resource
    {
        /* Note: storing registers in string for more flexibility should I need to refactor and or change the design. */
        public static string SmartAttributes =
@"0x00,Invalid
0x01,Raw read error rate
0x02,Throughput performance
0x03,Spinup time
0x04,Start/Stop count
0x05,Reallocated sector count
0x06,Read channel margin
0x07,Seek error rate
0x08,Seek timer performance
0x09,Power-on hours count
0x0A,Spinup retry count
0x0B,Calibration retry count
0x0C,Power cycle count
0x0D,Soft read error rate
0xAA,Available reserved space
0xAB,Program fail count
0xAC,Erase fail block count
0xAD,Wear level count
0xAE,Unexpected power loss count
0xB7,SATA downshift count
0xB8,End-to-End error
0xBB,Uncorrectable error count
0xBE,Airflow Temperature
0xBF,G-sense error rate
0xC0,Unsafe shutdown count
0xC1,Load/Unload cycle count
0xC2,Temperature
0xC3,Hardware ECC recovered
0xC4,Reallocation count
0xC5,Current pending sector count
0xC6,Offline scan uncorrectable count
0xC7,Interface CRC error rate
0xC8,Write error rate
0xC9,Soft read error rate
0xCA,Data Address Mark errors
0xCB,Run out cancel
0xCC,Soft ECC correction
0xCD,Thermal asperity rate (TAR)
0xCE,Flying height
0xCF,Spin high current
0xD0,Spin buzz
0xD1,Offline seek performance
0xDC,Disk shift
0xDD,G-sense error rate
0xDE,Loaded hours
0xDF,Load/unload retry count
0xE0,Load friction
0xE1,Host writes
0xE2,Timer workload media wear
0xE3,Timer workload read/write ratio
0xE4,Timer workload timer
0xE6,GMR head amplitude
0xE7,Temperature
0xE8,Available reserved space
0xE9,Media wearout indicator
0xF0,Head flying hours
0xF1,Lift time writes
0xF2,Lift time reads
0xF9,Lift time writes (NAND)
0xFA,Read error retry rate";
    }
}
