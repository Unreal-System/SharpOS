/**
 *  (C) 2006-2007 The SharpOS Project Team - http://www.sharpos.org
 * 
 *  Licensed under the terms of the GNU GPL License version 2.
 * 
 *  Author: Mircea-Cristian Racasan <darx_kies@gmx.net>
 * 
 */

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

namespace SharpOS.AOT.X86
{
    public class TR
    {
        public static readonly TRType TR3 = new TRType("TR3", 3);
        public static readonly TRType TR4 = new TRType("TR4", 4);
        public static readonly TRType TR5 = new TRType("TR5", 5);
        public static readonly TRType TR6 = new TRType("TR6", 6);
        public static readonly TRType TR7 = new TRType("TR7", 7);

        public static TRType GetByID(string id)
        {
            if (id.Equals("null") == true)
            {
                return null;
            }

            switch (id.Substring(id.Length - 3))
            {
                case "TR3":
                    return TR.TR3;
                case "TR4":
                    return TR.TR4;
                case "TR5":
                    return TR.TR5;
                case "TR6":
                    return TR.TR6;
                case "TR7":
                    return TR.TR7;
                default:
                    throw new Exception("Unknown TR Register '" + id + "'");
            }
        }
    }
}