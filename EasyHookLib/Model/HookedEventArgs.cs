using System;
using System.Collections.Generic;

namespace EasyHookLib.Model
{
    public class HookedEventArgs: EventArgs
    {
        public Dictionary<string, object> Entries { get; }=new Dictionary<string, object>();

        public HookedEventArgs(params Tuple<string, object>[] args)
        {
            foreach (var tuple in args)
            {
                Entries.Add(tuple.Item1, tuple.Item2);
            }
        }

    }
}