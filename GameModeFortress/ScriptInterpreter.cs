﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Jint;
using Jint.Native;

namespace GameModeFortress
{
   public interface IScriptInterpreter {
      TimeSpan ExecutionTimeout { get; set; }
      bool Execute(string script);
      bool Execute(string script, out object result);
   }

   public class JavaScriptInterpreter : IScriptInterpreter
   {
      private JintEngine m_engine;
      private JsScope m_global_scope;

      public JavaScriptInterpreter()
      {
         Console.Write("Loading JavaScript interpreter: ");
         try
         {
            m_engine = new JintEngine();
            Console.WriteLine("done.");
         }
         catch (Exception e)
         {
            Console.WriteLine("FAIL.");
            Console.WriteLine("Unable to load JavaScript engine\n*********************************************\n\t"+e.Message+"\n*********************************************");
         }
      }

      public TimeSpan ExecutionTimeout { get; set; }

      public bool Execute(string script)
      {
         object result; // <-- discard
         return Execute(script, out result);
      }

      public bool Execute(string script, out object result)
      {
         try
         {
            result = m_engine.Run(script);
         }
         catch (Exception e)
         {
            result = null;
            Console.WriteLine("Script failed with error:\n*********************************************\n\t" + e.InnerException.Message + "\n*********************************************"+e.StackTrace);            
            return false;
         }
         return true;
      }
   }
}
