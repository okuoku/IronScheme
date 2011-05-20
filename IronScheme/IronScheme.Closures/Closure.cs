#region License
/* Copyright (c) 2007,2008,2009,2010,2011 Llewellyn Pritchard 
 * All rights reserved.
 * This source code is subject to terms and conditions of the BSD License.
 * See docs/license.txt. */
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Scripting;

namespace IronScheme.Runtime
{
  public delegate object ConsFromArrayHandler(object[] args);
  public delegate object[] ArrayFromConsHandler(object args);
  public delegate object AssertHandler(object who, object msg, params object[] irritants);


  

  [Serializable]
  public abstract class Closure : Callable
  {

    protected static object CheckCodeContext(CodeContext cc)
    {
      if (cc != null && cc.Scope.Parent.Parent == null)
      {
        return null;
      }
      return cc;
    }

    readonly static Dictionary<Type, int> targetmap = new Dictionary<Type, int>();

    static Closure()
    {
      targetmap.Add(typeof(CallTarget0), 0);
      targetmap.Add(typeof(CallTarget1), 1);
      targetmap.Add(typeof(CallTarget2), 2);
      targetmap.Add(typeof(CallTarget3), 3);
      targetmap.Add(typeof(CallTarget4), 4);
      targetmap.Add(typeof(CallTarget5), 5);
      targetmap.Add(typeof(CallTarget6), 6);
      targetmap.Add(typeof(CallTarget7), 7);
      targetmap.Add(typeof(CallTarget8), 8);
      targetmap.Add(typeof(CallTargetN), -1);

      targetmap.Add(typeof(CallTargetWithContext0), 0 + 16);
      targetmap.Add(typeof(CallTargetWithContext1), 1 + 16);
      targetmap.Add(typeof(CallTargetWithContext2), 2 + 16);
      targetmap.Add(typeof(CallTargetWithContext3), 3 + 16);
      targetmap.Add(typeof(CallTargetWithContext4), 4 + 16);
      targetmap.Add(typeof(CallTargetWithContext5), 5 + 16);
      targetmap.Add(typeof(CallTargetWithContext6), 6 + 16);
      targetmap.Add(typeof(CallTargetWithContext7), 7 + 16);
      targetmap.Add(typeof(CallTargetWithContext8), 8 + 16);
      targetmap.Add(typeof(CallTargetWithContextN), -1 + 16);

      targetmap.Add(typeof(CallTargetWithContext0<>), 0 + 32);
      targetmap.Add(typeof(CallTargetWithContext1<>), 1 + 32);
      targetmap.Add(typeof(CallTargetWithContext2<>), 2 + 32);
      targetmap.Add(typeof(CallTargetWithContext3<>), 3 + 32);
      targetmap.Add(typeof(CallTargetWithContext4<>), 4 + 32);
      targetmap.Add(typeof(CallTargetWithContext5<>), 5 + 32);
      targetmap.Add(typeof(CallTargetWithContext6<>), 6 + 32);
      targetmap.Add(typeof(CallTargetWithContext7<>), 7 + 32);
      targetmap.Add(typeof(CallTargetWithContext8<>), 8 + 32);
      targetmap.Add(typeof(CallTargetWithContextN<>), -1 + 32);
    }

    public static AssertHandler AssertionViolation;

    protected int paramcount = int.MaxValue;

    public static ConsFromArrayHandler ConsFromArray;
    public static ConsFromArrayHandler ConsStarFromArray;
    public static ArrayFromConsHandler ArrayFromCons;
    public static CallTarget2 Cons;
    public static Callable IdentityForCPS;
    public static object Unspecified;

    public override string ToString()
    {
      if (target == null || target.Method == null)
      {
        return "unknown closure";
      }
      string name = target.Method.Name;

      int i = name.IndexOf("::");
      if (i < 0)
      {
        return name;
      }
      return name.Substring(i + 2);
    }

    public override object Arity
    {
      get { return paramcount; }
    }

    public override object Form
    {
      get 
      {
        if (target == null || target.Method == null)
        {
          return Unspecified;
        }

        MethodInfo mi = target.Method;

        List<object> form = new List<object>();

        ParameterInfo[] pis = mi.GetParameters();

        form.Add( SymbolTable.StringToObject(ToString()));

        foreach (ParameterInfo pi in pis)
        {
          if (pi.ParameterType != typeof(CodeContext))
          {
            if (pi.ParameterType.IsArray)
            {
              form.Add(SymbolTable.StringToObject(pi.Name));
              return ConsStarFromArray(form.ToArray());
            }
            else
            {
              form.Add(SymbolTable.StringToObject(pi.Name));
            }
          }
        }

        return ConsFromArray(form.ToArray());
      }
    }

    [DebuggerStepThrough]
    public override object Call()
    {
      return Call(new object[0]);
    }

    [DebuggerStepThrough]
    public override object Call(object arg1)
    {
      return Call(new object[] { arg1 });
    }

    [DebuggerStepThrough]
    public override object Call(object arg1, object arg2)
    {
      return Call(new object[] { arg1, arg2 });
    }

    [DebuggerStepThrough]
    public override object Call(object arg1, object arg2, object arg3)
    {
      return Call(new object[] { arg1 , arg2, arg3 });
    }

    [DebuggerStepThrough]
    public override object Call(object arg1, object arg2, object arg3, object arg4)
    {
      return Call(new object[] { arg1 , arg2, arg3, arg4 });
    }

    [DebuggerStepThrough]
    public override object Call(object arg1, object arg2, object arg3, object arg4, object arg5)
    {
      return Call(new object[] { arg1, arg2, arg3, arg4, arg5 });
    }

    [DebuggerStepThrough]
    public override object Call(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6)
    {
      return Call(new object[] { arg1, arg2, arg3, arg4, arg5, arg6 });
    }

    [DebuggerStepThrough]
    public override object Call(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7)
    {
      return Call(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
    }

    [DebuggerStepThrough]
    public override object Call(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8)
    {
      return Call(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
    }

    //[DebuggerStepThrough]
    //public abstract object Call(object[] args);

    readonly Delegate target;

    readonly static MethodInfo[] None = { };

    public virtual MethodInfo[] Targets
    {
      get { return None; }
    }

    public virtual MethodInfo[] VarargTargets
    {
      get { return None; }
    }
    
    protected Closure() : this(null, -1)
    {
    }

    static bool IsValidParams(MethodInfo mi)
    {
      foreach (ParameterInfo pi in mi.GetParameters())
      {
        if (pi.ParameterType == typeof(CodeContext))
        {
          return false;
        }
        if (pi.ParameterType.IsArray && !pi.IsDefined(typeof(ParamArrayAttribute), false))
        {
          return false;
        }
      }
      return true;
    }

    protected static bool IsValid(MethodInfo mi)
    {
      return mi.IsPublic && mi.IsStatic && !mi.Name.Contains("#") && IsValidParams(mi) && mi.DeclaringType != null
#if DEBUG
        && mi.Module.Name != "<In Memory Module>"
#endif
        ;
    }

    protected Closure(Delegate target, int paramcount)
    {
      this.paramcount = paramcount;
      this.target = target;
    }

    protected object GetWho()
    {
      return SymbolTable.StringToObject(target.Method.Name);
    }

    [Serializable]
    sealed class ContextClosure : Closure
    {
      CodeContext cc;

      public ContextClosure(CodeContext cc, Delegate target, int paramcount)
        : base(target, paramcount)
      {
        this.cc = cc;
      }

      public override MethodInfo[] Targets
      {
        get { return IsValid(target.Method) ? new MethodInfo[]{ target.Method } : None; }
      }

      [DebuggerStepThrough]
      public override object Call(object[] args)
      {
        if (paramcount >= 0 && paramcount != args.Length)
        {
          AssertionViolation(GetWho(), string.Format("invalid argument count, expected {0} got {1}", paramcount, args.Length), args);
        }
        switch (paramcount)
        {
          case -1:
            return ((CallTargetWithContextN)target)(cc, args);
          case 0:
            return ((CallTargetWithContext0)target)(cc);
          case 1:
            return ((CallTargetWithContext1)target)(cc, args[0]);
          case 2:
            return ((CallTargetWithContext2)target)(cc, args[0], args[1]);
          case 3:
            return ((CallTargetWithContext3)target)(cc, args[0], args[1], args[2]);
          case 4:
            return ((CallTargetWithContext4)target)(cc, args[0], args[1], args[2], args[3]);
          case 5:
            return ((CallTargetWithContext5)target)(cc, args[0], args[1], args[2], args[3], args[4]);
          case 6:
            return ((CallTargetWithContext6)target)(cc, args[0], args[1], args[2], args[3], args[4], args[5]);
          case 7:
            return ((CallTargetWithContext7)target)(cc, args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
          case 8:
            return ((CallTargetWithContext8)target)(cc, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
          default:
            throw new NotSupportedException();
        }
      }

      [DebuggerStepThrough]
      public override object Call()
      {
        if (paramcount == 0)
        {
          return ((CallTargetWithContext0)target)(cc);
        }
        else
        {
          return base.Call();
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1)
      {
        if (paramcount == 1)
        {
          return ((CallTargetWithContext1)target)(cc, arg1);
        }
        else
        {
          return base.Call(arg1);
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1, object arg2)
      {
        if (paramcount == 2)
        {
          return ((CallTargetWithContext2)target)(cc, arg1, arg2);
        }
        else
        {
          return base.Call(arg1, arg2);
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1, object arg2, object arg3)
      {
        if (paramcount == 3)
        {
          return ((CallTargetWithContext3)target)(cc, arg1, arg2, arg3);
        }
        else
        {
          return base.Call(arg1, arg2, arg3);
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1, object arg2, object arg3, object arg4)
      {
        if (paramcount == 4)
        {
          return ((CallTargetWithContext4)target)(cc, arg1, arg2, arg3, arg4);
        }
        else
        {
          return base.Call(arg1, arg2, arg3, arg4);
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1, object arg2, object arg3, object arg4, object arg5)
      {
        if (paramcount == 5)
        {
          return ((CallTargetWithContext5)target)(cc, arg1, arg2, arg3, arg4, arg5);
        }
        else
        {
          return base.Call(arg1, arg2, arg3, arg4, arg5);
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6)
      {
        if (paramcount == 6)
        {
          return ((CallTargetWithContext6)target)(cc, arg1, arg2, arg3, arg4, arg5, arg6);
        }
        else
        {
          return base.Call(arg1, arg2, arg3, arg4, arg5, arg6);
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7)
      {
        if (paramcount == 7)
        {
          return ((CallTargetWithContext7)target)(cc, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
        else
        {
          return base.Call(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8)
      {
        if (paramcount == 8)
        {
          return ((CallTargetWithContext8)target)(cc, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
        else
        {
          return base.Call(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
      }
    }

    [Serializable]
    sealed class ContextClosure<CodeContext> : Closure
    {
      CodeContext cc;

      public ContextClosure(CodeContext cc, Delegate target, int paramcount)
        : base(target, paramcount)
      {
        this.cc = cc;
      }

      public override MethodInfo[] Targets
      {
        get { return IsValid(target.Method) ? new MethodInfo[] { target.Method } : None; }
      }

      [DebuggerStepThrough]
      public override object Call(object[] args)
      {
        if (paramcount >= 0 && paramcount != args.Length)
        {
          AssertionViolation(GetWho(), string.Format("invalid argument count, expected {0} got {1}", paramcount, args.Length), args);
        }
        switch (paramcount)
        {
          case -1:
            return ((CallTargetWithContextN<CodeContext>)target)(cc, args);
          case 0:
            return ((CallTargetWithContext0<CodeContext>)target)(cc);
          case 1:
            return ((CallTargetWithContext1<CodeContext>)target)(cc, args[0]);
          case 2:
            return ((CallTargetWithContext2<CodeContext>)target)(cc, args[0], args[1]);
          case 3:
            return ((CallTargetWithContext3<CodeContext>)target)(cc, args[0], args[1], args[2]);
          case 4:
            return ((CallTargetWithContext4<CodeContext>)target)(cc, args[0], args[1], args[2], args[3]);
          case 5:
            return ((CallTargetWithContext5<CodeContext>)target)(cc, args[0], args[1], args[2], args[3], args[4]);
          case 6:
            return ((CallTargetWithContext6<CodeContext>)target)(cc, args[0], args[1], args[2], args[3], args[4], args[5]);
          case 7:
            return ((CallTargetWithContext7<CodeContext>)target)(cc, args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
          case 8:
            return ((CallTargetWithContext8<CodeContext>)target)(cc, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
          default:
            throw new NotSupportedException();
        }
      }

      [DebuggerStepThrough]
      public override object Call()
      {
        if (paramcount == 0)
        {
          return ((CallTargetWithContext0<CodeContext>)target)(cc);
        }
        else
        {
          return base.Call();
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1)
      {
        if (paramcount == 1)
        {
          return ((CallTargetWithContext1<CodeContext>)target)(cc, arg1);
        }
        else
        {
          return base.Call(arg1);
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1, object arg2)
      {
        if (paramcount == 2)
        {
          return ((CallTargetWithContext2<CodeContext>)target)(cc, arg1, arg2);
        }
        else
        {
          return base.Call(arg1, arg2);
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1, object arg2, object arg3)
      {
        if (paramcount == 3)
        {
          return ((CallTargetWithContext3<CodeContext>)target)(cc, arg1, arg2, arg3);
        }
        else
        {
          return base.Call(arg1, arg2, arg3);
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1, object arg2, object arg3, object arg4)
      {
        if (paramcount == 4)
        {
          return ((CallTargetWithContext4<CodeContext>)target)(cc, arg1, arg2, arg3, arg4);
        }
        else
        {
          return base.Call(arg1, arg2, arg3, arg4);
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1, object arg2, object arg3, object arg4, object arg5)
      {
        if (paramcount == 5)
        {
          return ((CallTargetWithContext5<CodeContext>)target)(cc, arg1, arg2, arg3, arg4, arg5);
        }
        else
        {
          return base.Call(arg1, arg2, arg3, arg4, arg5);
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6)
      {
        if (paramcount == 6)
        {
          return ((CallTargetWithContext6<CodeContext>)target)(cc, arg1, arg2, arg3, arg4, arg5, arg6);
        }
        else
        {
          return base.Call(arg1, arg2, arg3, arg4, arg5, arg6);
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7)
      {
        if (paramcount == 7)
        {
          return ((CallTargetWithContext7<CodeContext>)target)(cc, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
        else
        {
          return base.Call(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8)
      {
        if (paramcount == 8)
        {
          return ((CallTargetWithContext8<CodeContext>)target)(cc, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
        else
        {
          return base.Call(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
      }
    }




    [Serializable]
    sealed class SimpleClosure : Closure
    {
      public SimpleClosure(Delegate target, int paramcount)
        : base(target, paramcount)
      {
      }

      public override MethodInfo[] Targets
      {
        get { return IsValid(target.Method) ? new MethodInfo[] { target.Method } : None; }
      }

      [DebuggerStepThrough]
      public override object Call(object[] args)
      {
        if (paramcount >= 0 && paramcount != args.Length)
        {
          AssertionViolation(GetWho(), string.Format("invalid argument count, expected {0} got {1}", paramcount, args.Length), args);
        }
        switch (paramcount)
        {
          case -1:
            return ((CallTargetN)target)(args);
          case 0:
            return ((CallTarget0)target)();
          case 1:
            return ((CallTarget1)target)(args[0]);
          case 2:
            return ((CallTarget2)target)(args[0], args[1]);
          case 3:
            return ((CallTarget3)target)(args[0], args[1], args[2]);
          case 4:
            return ((CallTarget4)target)(args[0], args[1], args[2], args[3]);
          case 5:
            return ((CallTarget5)target)(args[0], args[1], args[2], args[3], args[4]);
          case 6:
            return ((CallTarget6)target)(args[0], args[1], args[2], args[3], args[4], args[5]);
          case 7:
            return ((CallTarget7)target)(args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
          case 8:
            return ((CallTarget8)target)(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
          default:
            throw new NotSupportedException();
        }
      }

      [DebuggerStepThrough]
      public override object Call()
      {
        if (paramcount == 0)
        {
          return ((CallTarget0)target)();
        }
        else
        {
          return base.Call();
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1)
      {
        if (paramcount == 1)
        {
          return ((CallTarget1)target)(arg1);
        }
        else
        {
          return base.Call(arg1);
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1, object arg2)
      {
        if (paramcount == 2)
        {
          return ((CallTarget2)target)(arg1, arg2);
        }
        else
        {
          return base.Call(arg1, arg2);
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1, object arg2, object arg3)
      {
        if (paramcount == 3)
        {
          return ((CallTarget3)target)(arg1, arg2, arg3);
        }
        else
        {
          return base.Call(arg1, arg2, arg3);
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1, object arg2, object arg3, object arg4)
      {
        if (paramcount == 4)
        {
          return ((CallTarget4)target)(arg1, arg2, arg3, arg4);
        }
        else
        {
          return base.Call(arg1, arg2, arg3, arg4);
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1, object arg2, object arg3, object arg4, object arg5)
      {
        if (paramcount == 5)
        {
          return ((CallTarget5)target)(arg1, arg2, arg3, arg4, arg5);
        }
        else
        {
          return base.Call(arg1, arg2, arg3, arg4, arg5);
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6)
      {
        if (paramcount == 6)
        {
          return ((CallTarget6)target)(arg1, arg2, arg3, arg4, arg5, arg6);
        }
        else
        {
          return base.Call(arg1, arg2, arg3, arg4, arg5, arg6);
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7)
      {
        if (paramcount == 7)
        {
          return ((CallTarget7)target)(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
        else
        {
          return base.Call(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8)
      {
        if (paramcount == 8)
        {
          return ((CallTarget8)target)(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
        else
        {
          return base.Call(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
      }

    }

    public static Callable CreateStatic(Delegate target)
    {
      return Create(null, target);
    }

    public static Callable Create(CodeContext cc, Delegate target)
    {
      return Create((object)cc, target);
    }

    public static Callable Create(object cc, Delegate target)
    {
      int arity;
      if (targetmap.TryGetValue(target.GetType(), out arity))
      {
        if (arity < 15 && arity > -2) // no context
        {
          return new SimpleClosure(target, arity);
        }
        else if (arity < 31 && arity > 14)
        {
          arity -= 16;
          return new ContextClosure(cc as CodeContext, target, arity);
        }
        else
        {
          arity -= 32;
          var st = cc.GetType();
          var ct = typeof(ContextClosure<>).MakeGenericType(st);
          return Activator.CreateInstance(ct, cc, target, arity) as Callable;
        }
      }
      throw new NotSupportedException();
    }


    public static Callable CreateVarArgX(CodeContext cc, Delegate target, int paramcount)
    {
      return new VarArgClosure(cc, target, paramcount);
    }

    public static Callable CreateCase(CodeContext cc, Delegate[] targets, int[] arities)
    {
      return new CaseClosure(cc, targets, arities);
    }

    public static Callable CreateTypedCase(CodeContext cc, Callable[] targets, int[] arities)
    {
      return new CaseClosure(cc, targets, arities);
    }


    [Serializable]
    sealed class VarArgClosure : Closure
    {
      Callable realtarget;
      int pcount = 0;

      public VarArgClosure(CodeContext cc, Delegate target, int paramcount)
        : base(target, -1)
      {
        pcount = paramcount;
        realtarget = Create(cc, target) as Callable;
      }

      public override object Arity
      {
        get { return (double)( pcount - 1); }
      }

      public override MethodInfo[] VarargTargets
      {
        get { return IsValid(target.Method) ? new MethodInfo[] { target.Method } : None; }
      }

      public override object Form
      {
        get
        {
          if (target == null || target.Method == null)
          {
            return Unspecified;
          }

          MethodInfo mi = target.Method;

          List<object> form = new List<object>();

          ParameterInfo[] pis = mi.GetParameters();

          form.Add(SymbolTable.StringToObject(ToString()));

          foreach (ParameterInfo pi in pis)
          {
            if (pi.ParameterType != typeof(CodeContext))
            {
              form.Add(SymbolTable.StringToObject(pi.Name));
            }
          }

          return ConsStarFromArray(form.ToArray());
        }
      }

      [DebuggerStepThrough]
      public override object Call(object[] args)
      {
        if (args.Length < (double)Arity)
        {
          AssertionViolation(realtarget.ToString(), string.Format("invalid argument count, expected at least {0} got {1}", Arity, args.Length), args);
        }
        object[] newargs = new object[pcount];
        Array.Copy(args, newargs, pcount - 1);
        object[] last = new object[args.Length - pcount + 1];
        Array.Copy(args, pcount - 1, last, 0, last.Length);
        newargs[pcount - 1] = ConsFromArray(last);
        return realtarget.Call(newargs);
      }

      public override string ToString()
      {
        return realtarget.ToString();
      }
    }


    [Serializable]
    sealed class CaseClosure : Closure
    {
      int[] arities;
      List<Callable> targets = new List<Callable>();

      public override MethodInfo[] Targets
      {
        get 
        {
          List<MethodInfo> mis = new List<MethodInfo>();
          foreach (Closure c in targets)
          {
            mis.AddRange(c.Targets);
          }
          return mis.ToArray(); 
        }
      }

      public override MethodInfo[] VarargTargets
      {
        get
        {
          List<MethodInfo> mis = new List<MethodInfo>();
          foreach (Closure c in targets)
          {
            mis.AddRange(c.VarargTargets);
          }
          return mis.ToArray();
        }
      }

      public CaseClosure(CodeContext cc, Callable[] targets, int[] arities)
        : base(null, -1)
      {
        this.arities = arities;
        for (int i = 0; i < targets.Length; i++)
        {
          this.targets.Add(targets[i]);
        }
      }

      public CaseClosure(CodeContext cc, Delegate[] targets, int[] arities)
        : base(null, -1)
      {
        this.arities = arities;
        for (int i = 0; i < targets.Length; i++)
        {
          if (arities[i] < 0)
          {
            this.targets.Add(CreateVarArgX(cc, targets[i], -arities[i]));
          }
          else
          {
            this.targets.Add(Create(cc, targets[i]));
          }
        }
      }

      public override object Arity
      {
        get 
        {
          List<object> arities = new List<object>();
          foreach (Callable c in targets)
          {
            if (c.Arity != Unspecified)
            {
              arities.Add(c.Arity);
            }
          }
          switch (arities.Count)
          {
            case 1:
              return arities[0];
            default:
              return ConsFromArray(arities.ToArray());
          }
        }
      }

      public override object Form
      {
        get
        {
          List<object> forms = new List<object>();
          foreach (Callable c in targets)
          {
            if (c.Form != Unspecified)
            {
              forms.Add(c.Form);
            }
          }
          switch (forms.Count)
          {
            case 0:
              return Unspecified;
            case 1:
              return arities[0];
            default:
              return new MultipleValues(forms.ToArray());
          }
        }
      }

      public override string ToString()
      {
        foreach (Callable c in targets)
        {
          return c.ToString();
        }
        return "empty case-lambda";
      }

      [DebuggerStepThrough]
      public override object Call(object[] args)
      {
        int arglen = args.Length;

        for (int i = 0; i < arities.Length; i++)
        {
          int a = arities[i];
          if (a == arglen || (a < 0 && arglen >= -a - 1))
          {
            return targets[i].Call(args);
          }
        }


        object arts = ConsFromArray(Array.ConvertAll<int, object>(arities, x => x));

        return AssertionViolation(ToString(), "invalid argument count", args, arts);
      }

      [DebuggerStepThrough]
      public override object Call()
      {
        int i = Array.IndexOf(arities, 0);
        if (i >= 0)
        {
          return targets[i].Call();
        }
        else
        {
          return Call(new object[0]);
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1)
      {
        int i = Array.IndexOf(arities, 1);
        if (i >= 0)
        {
          return targets[i].Call(arg1);
        }
        else
        {
          return Call(new object[] { arg1 });
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1, object arg2)
      {
        int i = Array.IndexOf(arities, 2);
        if (i >= 0)
        {
          return targets[i].Call(arg1, arg2);
        }
        else
        {
          return Call(new object[] { arg1, arg2 });
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1, object arg2, object arg3)
      {
        int i = Array.IndexOf(arities, 3);
        if (i >= 0)
        {
          return targets[i].Call(arg1, arg2, arg3);
        }
        else
        {
          return Call(new object[] { arg1, arg2, arg3 });
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1, object arg2, object arg3, object arg4)
      {
        int i = Array.IndexOf(arities, 4);
        if (i >= 0)
        {
          return targets[i].Call(arg1, arg2, arg3, arg4);
        }
        else
        {
          return Call(new object[] { arg1, arg2, arg3, arg4 });
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1, object arg2, object arg3, object arg4, object arg5)
      {
        int i = Array.IndexOf(arities, 5);
        if (i >= 0)
        {
          return targets[i].Call(arg1, arg2, arg3, arg4, arg5);
        }
        else
        {
          return Call(new object[] { arg1, arg2, arg3, arg4, arg5 });
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6)
      {
        int i = Array.IndexOf(arities, 6);
        if (i >= 0)
        {
          return targets[i].Call(arg1, arg2, arg3, arg4, arg5, arg6);
        }
        else
        {
          return Call(new object[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7)
      {
        int i = Array.IndexOf(arities, 7);
        if (i >= 0)
        {
          return targets[i].Call(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
        else
        {
          return Call(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7 });
        }
      }

      [DebuggerStepThrough]
      public override object Call(object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8)
      {
        int i = Array.IndexOf(arities, 8);
        if (i >= 0)
        {
          return targets[i].Call(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
        else
        {
          return Call(new object[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 });
        }
      }

    }

#if CPS
    internal sealed class CPSClosure : Closure
    {
      readonly BuiltinMethod prim;

      public CPSClosure(BuiltinMethod prim)
      {
        this.prim = prim;
      }

      public override string ToString()
      {
        return prim.ToString();
      }

      public override object Call(object[] args)
      {
        object r = null, except = null;
        Callable K = args[0] as Callable;
        List<object> newargs = new List<object>();
        newargs.AddRange(args);
        newargs.RemoveAt(0);

        try
        {
          r = prim.Call(newargs.ToArray());
        }
        catch (Exception ex)
        {
          except = ex;
        }

        if (except == null)
        {
          return K.Call(r);
        }
        else
        {
          ICallable raise = OptimizedBuiltins.SymbolValue(SymbolTable.StringToObject("raise")) as ICallable;
          return OptimizedBuiltins.CallWithK(raise, K, except);
        }
      }
    }

#endif

  }


}
