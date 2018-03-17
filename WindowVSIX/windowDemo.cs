using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using EnvDTE;
using Expression = EnvDTE.Expression;

namespace WindowVSIX
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    /// 
    [Guid("9bacdd1d-f9e6-4205-a647-08a2336b7977")]
    public class windowDemo : ToolWindowPane
    {
        private windowDemoControl wControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="windowDemo"/> class.
        /// </summary>
        public windowDemo() : base(null)
        {
            this.Caption = "windowDemo";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            wControl = new windowDemoControl();
            wControl.button1.Click += Button1_Click;
            this.Content = wControl;
            
            dte = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(DTE)) as DTE;
            dte.Events.DebuggerEvents.OnContextChanged +=
                new _dispDebuggerEvents_OnContextChangedEventHandler(DebuggerEvents_OnContextChanged);
            //dte.Events.DebuggerEvents.OnEnterBreakMode += DebuggerEvents_OnEnterBreakMode;
            UpdateRegisters();
        }

        private void Button1_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            UpdateRegisters();
        }

        private void DebuggerEvents_OnEnterBreakMode(dbgEventReason Reason, ref dbgExecutionAction ExecutionAction)
        {
            UpdateRegisters();
        }

        private DTE dte;
        

        void DebuggerEvents_OnContextChanged(Process NewProcess, Program NewProgram, Thread NewThread,
            StackFrame NewStackFrame)
        {
            UpdateRegisters();
        }

        public void UpdateRegisters()
        {
            //var locWind = dte.Windows.Item("Locals");

            //if (locWind == null) return;
            if (dte.Debugger.CurrentStackFrame == null) return;
            
            Expressions generalRegisters = dte.Debugger.CurrentStackFrame.Locals/*.GetExpression("General Registers", true, 1).DataMembers*/;
            Expressions specialRegisters = dte.Debugger.CurrentStackFrame.Arguments;//GetExpression("Special Registers", true, 1).DataMembers;

            //ushort[] registers = new ushort[26];
            //string[] registers = new string[26];

            StringBuilder stringBuilder = new StringBuilder();

            //int currentRegister = 0;
            foreach (Expression gr in generalRegisters)
            {
                
                stringBuilder.Append($"♦ {gr.Name} ({gr.Type}) = {gr.Value} {gr.IsValidValue}\n");


                object u = new object();
                //Object.ReferenceEquals()

                Expression e = dte.Debugger.GetExpression($"{gr.Name}.GetHashCode()");
                stringBuilder.Append($"[{e.Name}=({e.Type})={e.Value}]\n");

                Expression e2 = dte.Debugger.GetExpression($"LocalPresenter.PresenterHelper.GetAddress({gr.Name})");
                stringBuilder.Append($"[{e2.Name}=({e2.Type})={e2.Value}]\n");

                foreach (Expression grDataMember in gr.DataMembers)
                {


                    stringBuilder.Append($"\t♥ {grDataMember.Name} ({grDataMember.Type}) =" +
                                         $" {grDataMember.Value}\n");

                    Expression innerE = dte.Debugger.GetExpression($"{gr.Name}.{grDataMember.Name}.GetHashCode()");
                    stringBuilder.Append($"\t  [{innerE.Name}=({innerE.Type})={innerE.Value}]\n");

                    Expression e22 = dte.Debugger.GetExpression($"LocalPresenter.PresenterHelper.GetAddress({gr.Name}.{grDataMember.Name})");
                    stringBuilder.Append($"[{e22.Name}=({e22.Type})={e22.Value}]\n");
                }

                //foreach (Expression expression in gr.Collection)
                //{
                //    stringBuilder.Append($"\t ♦ {expression.Name} ({expression.Type}) =" +
                //                         $" {expression.Value} {expression.IsValidValue}\n");
                //}

                ////registers[currentRegister++] = Convert.ToUInt16(gr.Value, 16);
                //registers[currentRegister++] = gr.Name; //.Value;
                //registers[currentRegister++] = gr.Type;
                //registers[currentRegister++] = gr.Value;
            }

            //foreach (Expression sr in specialRegisters)
            //{
            //    //ushort test = Convert.ToUInt16(sr.Value, 16);
            //    //registers[currentRegister++] = Convert.ToUInt16(sr.Value, 16);
            //    //registers[currentRegister++] = sr.Name; //.Value;

            //    stringBuilder.Append($"=={sr.Name} ({sr.Type}) = {sr.Value} {sr.IsValidValue}\n");
            //    foreach (Expression grDataMember in sr.DataMembers)
            //    {

            //        stringBuilder.Append($"==\t ♥ {grDataMember.Name} ({grDataMember.Type}) =" +
            //                             $" {grDataMember.Value} {grDataMember.IsValidValue}\n");
            //    }

            //    //foreach (Expression expression in sr.Collection)
            //    //{
            //    //    stringBuilder.Append($"==\t ♦ {expression.Name} ({expression.Type}) =" +
            //    //                         $" {expression.Value} {expression.IsValidValue}\n");
            //    //}
            //}

            //this.Caption = string.Join("; ",registers.ToString()); 
            wControl.CleanLog();
            wControl.AddLog(stringBuilder.ToString());
        }
    }
}
