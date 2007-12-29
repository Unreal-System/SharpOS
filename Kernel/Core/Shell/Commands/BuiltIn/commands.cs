// 
// (C) 2007 The SharpOS Project Team (http://www.sharpos.org)
//
// Authors:
//	Bruce Markham <illuminus86@gmail.com>
//
// Licensed under the terms of the GNU GPL v3,
//  with Classpath Linking Exception for Libraries
//

using System;
using System.Collections.Generic;
using System.Text;
using SharpOS.AOT.Attributes;
using SharpOS.Foundation;
using SharpOS.ADC;

namespace SharpOS.Shell.Commands.BuiltIn
{
    public unsafe static class commands
    {
        public const string name = "commands";
        public const string shortDescription = "Displays a list of all the commands";
        public const string lblExecute = "COMMANDS.commands.Execute";
        public const string lblGetHelp = "COMMANDS.commands.GetHelp";

        [Label(lblExecute)]
        public static void Execute(CommandExecutionContext* context)
        {
            Prompter.DisplayCommandList();
        }

        [Label(lblGetHelp)]
        public static void GetHelp(CommandExecutionContext* context)
        {
            if (context->parameters == null ||
                context->parameters->Length == 0)
            {
                ADC.Memory.Call((void*)Stubs.GetFunctionPointer(lblExecute), (void*)context);
            }
            else
            {
                CommandExecutionAttemptResult result;
                result = Prompter.CommandTable->HandleLine(context->parameters, false, true);
                if (result == CommandExecutionAttemptResult.NotFound)
                {
                    int indexOfSpace = context->parameters->IndexOf(" ");
                    CString8* tempStr;
                    if (indexOfSpace >= 0)
                        tempStr = context->parameters->Substring(0, indexOfSpace);
                    else
                        tempStr = CString8.Copy(context->parameters);

                    TextMode.Write("No command '");
                    TextMode.Write(tempStr);
                    TextMode.WriteLine("' is available to retrieve help for.");
                    TextMode.WriteLine(CommandTableHeader.inform_USE_HELP_COMMANDS);

                    CString8.DISPOSE(tempStr);
                    return;
                }
                if (result == CommandExecutionAttemptResult.BlankEntry)
                {
                    ADC.Memory.Call((void*)Stubs.GetFunctionPointer(help.lblGetHelp), (void*)context);
                }

            }
        }

        public static CommandTableEntry* CREATE()
        {
            CommandTableEntry* entry = (CommandTableEntry*)SharpOS.ADC.MemoryManager.Allocate((uint)sizeof(CommandTableEntry));

            entry->name = (CString8*)SharpOS.Stubs.CString(name);
            entry->shortDescription = (CString8*)SharpOS.Stubs.CString(shortDescription);
            entry->func_Execute = (void*)SharpOS.Stubs.GetLabelAddress(lblExecute);
            entry->func_GetHelp = (void*)SharpOS.Stubs.GetLabelAddress(lblGetHelp);
            entry->nextEntry = null;

            return entry;
        }
    }
}