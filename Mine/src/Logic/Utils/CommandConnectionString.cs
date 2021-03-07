using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Utils
{
    public sealed class CommandConnectionString
    {
        public CommandConnectionString(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }

    public sealed class QueriesConnectionString
    {
        public QueriesConnectionString(string va lue)
        {
            Value = value;
        }

        public string Value { get; }
    }
}
