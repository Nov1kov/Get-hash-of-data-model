using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using AutoBogus;
using Bogus.Platform;

namespace SharpObjectGenerator.AutoBogusExtensions
{
    public class SkipNoSerializeBinder : AutoBinder
    {
        public override Dictionary<string, MemberInfo> GetMembers(Type t)
        {
            var group = t.GetAllMembers(BindingFlags)
                .Where(m =>
                {
                    if( m.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Any() )
                    {
                        return false;
                    }
                    
                    var noSerialAttr = m.GetCustomAttribute<NonSerializedAttribute>();
                    if (noSerialAttr != null)
                    {
                        return false;
                    }
                    
                    if( m is PropertyInfo pi )
                    {
                        return pi.CanWrite;
                    }

                    if (m is FieldInfo)
                    {
                        // revert the fix 
                        // https://github.com/bchavez/Bogus/commit/1a746be324b35f776ad9c8ed74aea6611764d94c
                        return true;
                    }
                    return false;
                })
                .GroupBy(mi => mi.Name);

            return group.ToDictionary(k => k.Key, g => g.First());
        }
    }
}