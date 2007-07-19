// 
// (C) 2006-2007 The SharpOS Project Team (http://www.sharpos.org)
//
// Authors:
//	Mircea-Cristian Racasan <darx_kies@gmx.net>
//
// Licensed under the terms of the GNU GPL License version 2.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using SharpOS;
using SharpOS.AOT.X86;
using SharpOS.AOT.IR;

namespace SharpOS.ADC.X86 {
	public unsafe class GDT {
		private const ushort GDTEntries = 3;
		private const ushort SystemSelector = 0;
		public const ushort CodeSelector = 8;
		public const ushort DataSelector = 16;

		private static DTPointer* gdtPointer = (DTPointer*) Kernel.LabelledAlloc ("GDTPointer", DTPointer.SizeOf);
		private static Entry* gdt = (Entry*) Kernel.Alloc (Entry.SizeOf * GDTEntries);

		[StructLayout (LayoutKind.Sequential)]
		public struct Entry {
			public enum Type : ushort {
				Accessed = 1,
				Writable = 2,
				Expansion = 4,
				Executable = 8,
				Descriptor = 16,
				Privilege_Ring_0 = 0,
				Privilege_Ring_1 = 32,
				Privilege_Ring_2 = 64,
				Privilege_Ring_3 = 96,
				Present = 128,
				OperandSize_16Bit = 0,
				OperandSize_32Bit = 1024,
				Granularity_Byte = 0,
				Granularity_4K = 2048
			}

			public const uint SizeOf = 8;

			public ushort LimitLow;
			public ushort BaseLow;
			public byte BaseMiddle;
			public byte Access;
			public byte Granularity;
			public byte BaseHigh;

			public void Setup (uint _base, uint _limit, ushort flags)
			{
				this.BaseLow = (ushort) (_base & 0xFFFF);
				this.BaseMiddle = (byte) ((_base >> 16) & 0xFF);
				this.BaseHigh = (byte) ((_base >> 24) & 0xFF);

				// The limits
				this.LimitLow = (ushort) (_limit & 0xFFFF);
				this.Granularity = (byte) ((_limit >> 16) & 0x0F);

				// Granularity and Access
				this.Granularity |= (byte) ((flags >> 4) & 0xF0);
				this.Access = (byte) (flags & 0xFF);
			}
		}

		internal static void Setup ()
		{
			gdtPointer->Setup ((ushort) (sizeof (Entry) * GDTEntries - 1), (uint) gdt);

			Screen.WriteMessage (Kernel.String ("GDT Pointer: 0x"));
			Screen.WriteNumber (true, (int) gdtPointer->Address);
			Screen.WriteMessage (Kernel.String (" - 0x"));
			Screen.WriteNumber (true, gdtPointer->Size);
			Screen.WriteNL ();

			gdt [SystemSelector >> 3].Setup (0, 0, 0);

			// Code Segment
			gdt [CodeSelector >> 3].Setup (0, 0xFFFFFFFF, (ushort) (
				Entry.Type.Granularity_4K |
				Entry.Type.OperandSize_32Bit |
				Entry.Type.Present |
				Entry.Type.Descriptor |
				Entry.Type.Executable |
				Entry.Type.Writable));

			// Data Segment
			gdt [DataSelector >> 3].Setup (0, 0xFFFFFFFF, (ushort) (
				Entry.Type.Granularity_4K |
				Entry.Type.OperandSize_32Bit |
				Entry.Type.Present |
				Entry.Type.Descriptor |
				Entry.Type.Writable));

			Asm.LGDT (new Memory ("GDTPointer"));

			Asm.MOV (R16.AX, DataSelector);
			Asm.MOV (Seg.DS, R16.AX);
			Asm.MOV (Seg.ES, R16.AX);
			Asm.MOV (Seg.FS, R16.AX);
			Asm.MOV (Seg.GS, R16.AX);
			Asm.MOV (Seg.SS, R16.AX);

			Asm.JMP (CodeSelector, "Kernel_GDT_Entry_Point");
			Asm.LABEL ("Kernel_GDT_Entry_Point");
		}
	}
}
