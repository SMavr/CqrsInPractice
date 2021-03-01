using System;

namespace Logic.Students
{
    public class Messages
    {
        private readonly IServiceProvider provider;

        public Messages(IServiceProvider provider)
        {
            this.provider = provider;
        }
    }
}
