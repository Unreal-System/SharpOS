// 
// (C) 2006-2007 The SharpOS Project Team (http://www.sharpos.org)
//
// Authors:
//	Mircea-Cristian Racasan <darx_kies@gmx.net>
//
// Licensed under the terms of the GNU GPL License version 2.
//

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using SharpOS.AOT.IR;
using SharpOS.AOT.IR.Instructions;
using SharpOS.AOT.IR.Operands;
using SharpOS.AOT.IR.Operators;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;

namespace SharpOS.AOT.X86 {
	internal class Bits32Instruction : Instruction {
		/// <summary>
		/// Initializes a new instance of the <see cref="Bits32Instruction"/> class.
		/// </summary>
		/// <param name="value">if set to <c>true</c> [value].</param>
		public Bits32Instruction (bool value)
			: base (true, string.Empty, string.Empty, "[BITS", value ? "32]" : "16]", null, null, null, value, null)
		{
		}
	}
}